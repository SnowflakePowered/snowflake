import { Game } from "../remoting/Games"

export interface Action<T> {
    type: string
    payload: T
    error?: boolean
}

export const REQUEST_GAMES = "REQUEST_GAMES"
export const RECEIVE_GAMES = "RECEIVE_GAMES"

export const requestGames = (games: Game[]) : Action<Game[]> => {
    return {
        type: REQUEST_GAMES,
        payload: games
    }
}

export const receiveGames = (games: Game[]) : Action<Game[]> => {
    return {
        type: RECEIVE_GAMES,
        payload: games
    }
}
