export default (state = {}, payload) => {
  switch (payload.type) {
    case 'UPDATE_GAMES':
      return payload.games
    default:
      return state
  }
}
