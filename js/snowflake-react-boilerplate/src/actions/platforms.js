import { UPDATE_PLATFORMS, createAction } from './actions'

export const updatePlatforms = (platforms) => createAction(UPDATE_PLATFORMS)(platforms)

export const beginUpdatePlatforms = () => {
  return async (dispatch, getState, snowflake) => {
    const platforms = await snowflake.stone.getPlatforms()
    dispatch(updatePlatforms(platforms))
  }
}

