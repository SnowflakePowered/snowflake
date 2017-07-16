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

  public getPlatforms = async (): Promise<Map<string, Platform>> => {
    const platforms = await request<Platform[]>(this.getServiceUrl('platforms'))
    if (platforms.Error) { throw platforms.Error }
    const array = platforms.Response.map(platform => [platform.PlatformID, Immutable.from(platform)] as [string, Platform])
    return new Map(array)
  }
}
