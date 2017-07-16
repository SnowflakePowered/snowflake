export const SNOWFLAKE_REFRESH_GAMES = 'SNOWFLAKE_REFRESH_GAMES'
export const SNOWFLAKE_REFRESH_PLATFORMS = 'SNOWFLAKE_REFRESH_PLATFORMS'
export const STATE_SET_ACTIVE_PLATFORM = 'STATE_SET_ACTIVE_PLATFORM'

export const createAction = (type) => {
  return (payload) => {
    return {
      type: type,
      payload: payload
    }
  }
}

const timeout = ms => new Promise(resolve => setTimeout(resolve, ms))

export const beginCreateAction = (type, delay = 0) => {
  return (payload) => {
    return async (dispatch) => {
      await timeout(delay)
      dispatch(createAction(type)(payload))
    }
  }
}
