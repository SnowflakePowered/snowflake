import * as Immutable from 'seamless-immutable'
import { request, Response, Service } from './Remoting'

export interface Platform {
  PlatformID: string
  Metadata: { [key: string]: string }
  MaximumInputs: number
  FriendlyName: string
  FileTypes: { [key: string]: string }
}

export class Stone extends Service {
  constructor(rootUrl: string) {
    super(rootUrl, 'stone')
  }

  public getPlatforms = async (): Promise<{[platformID: string]: Platform}> => {
    const platforms = await request<Platform[]>(this.getServiceUrl('platforms'))
    if (platforms.Status.Code >= 400 ) { throw new Error(platforms.Status.Message) }
    const array = platforms.Response
        .map(platform => ({ [platform.PlatformID] : Immutable.from(platform) }))
    return Immutable.from(Object.assign({}, ...array))
  }
}
