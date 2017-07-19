import { SNOWFLAKE_REFRESH_GAMES, createAction } from './'

export const refreshGames = games => createAction(SNOWFLAKE_REFRESH_GAMES)(games)

export const beginRefreshGames = () => {
  return async (dispatch, getState, snowflake) => {
    const games = await snowflake.games.getGames()
    dispatch(refreshGames(games))
  }
}
