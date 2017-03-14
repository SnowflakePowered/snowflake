import { UPDATE_GAMES } from "./actions"
export const updateGames = (games) => {
    return {
        type: UPDATE_GAMES,
        games
    }
}

export const beginUpdateGames = () => {  
  return async (dispatch, getState, snowflake) => {
    console.log(getState())
    const games = await snowflake.games.getGames()
    dispatch(updateGames(games))
  }
}

