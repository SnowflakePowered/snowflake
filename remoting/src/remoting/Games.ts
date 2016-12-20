import * as Immutable from "seamless-immutable"
import { Platform } from "./Stone"
import { Response, request } from "./Remoting"

export interface Game {
    Files: File[]
    Guid: string
    Metadata: { [key: string]: Metadata }
    PlatformId: string
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

export const getGames = async (): Promise<Iterable<Game>> => {
    let games = await request<Game[]>("http://localhost:9696/games")
    if (games.Error) { throw games.Error }
    return Immutable.from(games.Response)
}

export const getGame = async (uuid: string): Promise<Game> => {
    let game = await request<Game>(`http://localhost:9696/games/${uuid}`)
    if (game.Error) { throw game.Error }
    return Immutable.from(game.Response)
}

export const createGame = async (title: string, platform: Platform): Promise<Game> => {
    let game = await request<Game>("http://localhost:9696/games", {
        title,
        platform: platform.PlatformID
    }, "Create")
    if (game.Error) { throw game.Error }
    return Immutable.from(game.Response)
}

export const createFile = async (game: Game, path: string, mimetype: string) => {
    let newGame = await request<Game>(`http://localhost:9696/games/${game.Guid}/files`, { path, mimetype }, "Create")
    if (newGame.Error) { throw newGame.Error }
    return Immutable.from(newGame.Response)
}

