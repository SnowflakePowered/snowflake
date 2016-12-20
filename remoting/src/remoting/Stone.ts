import { Response, request } from "./Remoting"
import * as Immutable from "seamless-immutable"
export interface Platform {
    PlatformID: string
    Metadata: { [key: string]: string }
    MaximumInputs: number
    FriendlyName: string
    FileTypes: { [key: string]: string }
}

export async function getPlatforms(): Promise<Map<string, Platform>> {
    let platform = await request<Platform[]>("http://localhost:9696/stone/platforms")
    if (platform.Error) throw platform.Error
    let array = <[string, Platform][]>platform.Response.map(platform => [platform.PlatformID, Immutable.from(platform)])
    return new Map<string, Platform>(array)
}