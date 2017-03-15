export default (state = {}, action) => {
  switch (action.type) {
    case 'UPDATE_PLATFORMS':
      return action.payload
    default:
      return state
  }
}
