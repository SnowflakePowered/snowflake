import { Response, request } from "./Remoting"
import { Platform } from "./Stone"
import * as Immutable from "seamless-immutable"
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

export async function getGames(): Promise<Iterable<Game>> {
    let games = await request<Game[]>("http://localhost:9696/games")
    if (games.Error) throw games.Error
    return Immutable.from(games.Response)
}

export async function getGame(uuid: string): Promise<Game> {
    let game = await request<Game>(`http://localhost:9696/games/${uuid}`)
    if (game.Error) throw game.Error
    return Immutable.from(game.Response)
}

export async function createGame(title: string, platform: Platform): Promise<Game> {
    let game = await request<Game>("http://localhost:9696/games", { title: title, platform: platform.PlatformID }, "Create")
    if (game.Error) throw game.Error
    return Immutable.from(game.Response)
}

export async function createFile(game: Game, path: string, mimetype: string) {
    let newGame = await request<Game>(`http://localhost:9696/games/${game.Guid}/files`, { path: path, mimetype: mimetype }, "Create")
    if (newGame.Error) throw newGame.Error
    return Immutable.from(newGame.Response)
}

