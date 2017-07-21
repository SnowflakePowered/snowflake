import { Service, request } from './Remoting'
import { Game } from './Games'
import { ConfigurationCollection } from './Configuration'
import * as Immutable from 'seamless-immutable'

export class Emulators extends Service {
  constructor (rootUrl: string) {
    super(rootUrl, 'emulators')
  }

  public getConfiguration = async (emulatorName: string, gameUuid: string): Promise<ConfigurationCollection> => {
    const configuration = await request<ConfigurationCollection>(this.getServiceUrl(emulatorName, 'config', gameUuid))
    if (configuration.Status.Code >= 400 ) { throw new Error(configuration.Status.Message) }
    return Immutable.from(configuration.Response)
  }
}
