import { SNOWFLAKE_REFRESH_PLATFORMS } from '../actions/'

export default (state = {}, action) => {
  switch (action.type) {
    case SNOWFLAKE_REFRESH_PLATFORMS:
      return action.payload
    default:
      return state
  }
}
