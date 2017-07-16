import { SNOWFLAKE_REFRESH_PLATFORMS, createAction } from './'

export const refreshPlatforms = platforms => createAction(SNOWFLAKE_REFRESH_PLATFORMS)(platforms)

export const beginRefreshPlatforms = () => {
  return async (dispatch, getState, snowflake) => {
    const platforms = await snowflake.stone.getPlatforms()
    dispatch(refreshPlatforms(platforms))
  }
}

