import * as Immutable from 'seamless-immutable'
import { request, Response, Service } from './Remoting'
import { Platform } from './Stone'

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

  public getGames = async (): Promise<Iterable<Game>> => {
    const games = await request<Game[]>(this.getServiceUrl())
    if (games.Error) { throw games.Error }
    return Immutable.from(games.Response)
  }

  public getGame = async (uuid: string): Promise<Game> => {
    const game = await request<Game>(this.getServiceUrl(uuid))
    if (game.Error) { throw game.Error }
    return Immutable.from(game.Response)
  }

  public createGame = async (title: string, platform: Platform): Promise<Game> => {
    const game = await request<Game>(this.getServiceUrl(), {
      title,
      platform: platform.PlatformID
    }, 'Create')
    if (game.Error) { throw game.Error }
    return Immutable.from(game.Response)
  }

  public createFile = async (game: Game, path: string, mimetype: string) => {
    const newGame = await request<Game>(this.getServiceUrl(game.Guid, 'files'), { path, mimetype }, 'Create')
    if (newGame.Error) { throw newGame.Error }
    return Immutable.from(newGame.Response)
  }

}
