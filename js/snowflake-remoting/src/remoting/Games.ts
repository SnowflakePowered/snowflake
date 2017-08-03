import * as Immutable from 'seamless-immutable'
import { request, Response, Service } from './Remoting'
import { Platform } from './Stone'
import { ConfigurationCollection, ConfigurationOption, ConfigurationValue } from './Configuration'

export interface Game {
  Files: File[]
  Guid: string
  Metadata: { [key: string]: Metadata }
  PlatformID: string
  Title: string
}

export interface File {
  FilePath: string
  Guid: string
  Metadata: { [key: string]: Metadata }
  MimeType: string
  Record: string
}

export interface Metadata {
  Guid: string
  Key: string
  Record: string
  Value: string
}

export class Games extends Service {
  constructor(rootUrl: string) {
    super(rootUrl, 'games')
  }

  public getGames = async (): Promise<{[gameGuid: string]: Game}> => {
    const games = await request<Game[]>(this.getServiceUrl())
    if (games.Status.Code >= 400 ) { throw new Error(games.Status.Message) }
    const array = games.Response.map(game => ({ [game.Guid]: Immutable.from(game) }))
    return Immutable.from(Object.assign({}, ...array))
  }

  public getGame = async (uuid: string): Promise<Game> => {
    const game = await request<Game>(this.getServiceUrl(uuid))
    if (game.Status.Code >= 400 ) { throw new Error(game.Status.Message) }
    return Immutable.from(game.Response)
  }

  public createGame = async (title: string, platform: Platform): Promise<Game> => {
    const game = await request<Game>(this.getServiceUrl(), {
      title,
      platform: platform.PlatformID
    }, 'Create')
    if (game.Status.Code >= 400 ) { throw new Error(game.Status.Message) }
    return Immutable.from(game.Response)
  }

  public createFile = async (gameGuid: string, path: string, mimetype: string) => {
    const newGame = await request<Game>(this.getServiceUrl(gameGuid, 'files'), { path, mimetype }, 'Create')
    if (newGame.Status.Code >= 400 ) { throw new Error(newGame.Status.Message) }
    return Immutable.from(newGame.Response)
  }

  public getConfigurations = async (gameGuid: string, profileName?: string) => {
    profileName = profileName || 'default'
    const configurations = await request<{ [emulatorName: string]: ConfigurationCollection }>(this.getServiceUrl(gameGuid, 'configs', profileName))
    if (configurations.Status.Code >= 400 ) { throw new Error(configurations.Status.Message) }
    return Immutable.from(configurations.Response)
  }

  public getEmulatorConfigurations = async (gameGuid: string, profileName: string, emulator: string) => {
    const configurations = await request<ConfigurationCollection>(this.getServiceUrl(gameGuid, 'configs', profileName, emulator))
    if (configurations.Status.Code >= 400 ) { throw new Error(configurations.Status.Message) }
    return Immutable.from(configurations.Response)
  }

  public setEmulatorConfigurationValue = async  (gameGuid: string, profileName: string, emulator: string, newValue: ConfigurationValue) => {
    const configurations = await request<ConfigurationCollection>(this.getServiceUrl(gameGuid, 'configs', profileName, emulator),
                                 { valueGuid: newValue.Guid, newStrValue: newValue.Value }, 'Update')
    if (configurations.Status.Code >= 400 ) { throw new Error(configurations.Status.Message) }
    return Immutable.from(configurations.Response)
  }

  public setEmulatorConfigurationValues = async (gameGuid: string, profileName: string, emulator: string, newValues: ConfigurationValue[]) => {
    for (const value of newValues) {
      await this.setEmulatorConfigurationValue(gameGuid, profileName, emulator, value)
    }
    const refreshedConfig = await this.getEmulatorConfigurations(gameGuid, profileName, emulator)
    return refreshedConfig
  }
}
