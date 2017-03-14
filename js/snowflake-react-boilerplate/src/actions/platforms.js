import { UPDATE_PLATFORMS } from './actions'

export const updatePlatforms = (platforms) => {
  return {
    type: UPDATE_PLATFORMS,
    platforms
  }
}

export const beginUpdatePlatforms = () => {
  return async (dispatch, getState, snowflake) => {
    console.log(getState())
    const platforms = await snowflake.stone.getPlatforms()
    dispatch(updatePlatforms(platforms))
  }
}

