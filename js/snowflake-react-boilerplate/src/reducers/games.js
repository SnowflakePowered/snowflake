export default (state = {}, action) => {
  switch (action.type) {
    case 'UPDATE_GAMES':
      return action.payload
    default:
      return state
  }
}
