import { reducerWithInitialState } from 'typescript-fsa-reducers'
import State, { InitialState } from 'state/State'
import * as Actions from 'state/Actions'

const reducer = reducerWithInitialState<State>(InitialState)
  .case(Actions.refreshPlatforms.done, (action, payload) => {
    return {
      ...action,
      Platforms: payload.result
    }
  })
  .case(Actions.refreshGames.done, (action, payload) => {
    return {
      ...action,
      Games: payload.result
    }
  })
  .case(Actions.setActivePlatform, (action, payload) => {
    return {
      ...action,
      ActivePlatform: payload
    }
  })
  .case(Actions.setActiveGame, (action, payload) => {
    return {
      ...action,
      ActiveGame: payload
    }
  })
  .case(Actions.locationChange, (action, payload) => {
    return {
      ...action,
      location: payload
    }
  })
  .case(Actions.refreshActiveGameConfiguration.done, (action, payload) => {
    return {
      ...action,
      ActiveGameConfiguration: payload.result
    }
  })
  .build()

export default reducer
