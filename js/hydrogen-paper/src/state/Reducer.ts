import { reducerWithInitialState } from 'typescript-fsa-reducers'
import RootState, { InitialState } from './RootState'
import * as Actions from './Actions'

const reducer = reducerWithInitialState<RootState>(InitialState)
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
  .case(Actions.setCurrentPlatform, (action, payload) => {
    return {
      ...action,
      CurrentPlatform: payload
    }
  })
  .build()

export default reducer
