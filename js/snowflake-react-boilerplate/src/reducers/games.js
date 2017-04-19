import { SNOWFLAKE_REFRESH_GAMES } from '../actions/'

export default (state = {}, action) => {
  switch (action.type) {
    case SNOWFLAKE_REFRESH_GAMES:
      return action.payload
    default:
      return state
  }
}
