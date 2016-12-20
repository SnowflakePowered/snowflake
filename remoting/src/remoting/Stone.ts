import * as Immutable from "seamless-immutable"
import { request, Response } from "./Remoting"

export interface Platform {
    PlatformID: string
    Metadata: { [key: string]: string }
    MaximumInputs: number
    FriendlyName: string
    FileTypes: { [key: string]: string }
}

export const getPlatforms = async (): Promise<Map<string, Platform>> => {
    let platforms = await request<Platform[]>("http://localhost:9696/stone/platforms")
    if (platforms.Error) { throw platforms.Error }
    let array = platforms.Response.map(platform => [platform.PlatformID, Immutable.from(platform)]) as Array<[string, Platform]>
    return new Map<string, Platform>(array)
}
