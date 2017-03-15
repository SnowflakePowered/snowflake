export const UPDATE_GAMES = 'UPDATE_GAMES'
export const UPDATE_PLATFORMS = 'UPDATE_PLATFORMS'
export const createAction = (type) => {
  return (payload) => {
    return {
      type: type,
      payload: payload
    }
  }
}
