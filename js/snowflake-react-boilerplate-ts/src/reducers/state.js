import { STATE_SET_ACTIVE_PLATFORM } from '../actions/'

export default (state = {}, action) => {
  switch (action.type) {
    case STATE_SET_ACTIVE_PLATFORM:
      return {
        ...state,
        currentPlatform: action.payload
      }
    default:
      return state
  }
}
