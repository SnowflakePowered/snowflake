import { UI_SET_ACTIVE_PLATFORM } from '../actions/actions'


export default (state = {}, action) => {
  switch (action.type) {
    case UI_SET_ACTIVE_PLATFORM:
      return {
        ...state,
        activePlatform: action.payload
      }
    default:
      return state
  }
}
