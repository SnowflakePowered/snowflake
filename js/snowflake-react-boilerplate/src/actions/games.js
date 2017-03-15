import { UPDATE_GAMES, createAction } from './actions'

export const updateGames = (games) => createAction(UPDATE_GAMES)(games)

export const beginUpdateGames = () => {
  return async (dispatch, getState, snowflake) => {
    console.log(getState())
    const games = await snowflake.games.getGames()
    dispatch(updateGames(games))
  }
}

