import { reducerWithInitialState } from 'typescript-fsa-reducers'
import State, { InitialState } from 'state/State'
import * as Actions from 'state/Actions'
import { Map } from 'immutable'
import * as Immutable from 'immutable'
import { ConfigurationCollection } from 'snowflake-remoting'
import { ConfigurationKey } from 'support/ConfigurationKey'

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
  .case(Actions.retrieveGameConfiguration.done, (action, payload) => {
    const configs = payload.result
    const { gameGuid, profileName } = payload.params
    let test
    const mapEntries: [ConfigurationKey, ConfigurationCollection][] = Object.entries(configs).map(([emulatorName, config]) => {
      const key: ConfigurationKey = ConfigurationKey(gameGuid, emulatorName, profileName)
      const entry: [ConfigurationKey, ConfigurationCollection] = [Immutable.fromJS(key), config]
      test = key
      return entry
    })
    const newMap = Map<ConfigurationKey, ConfigurationCollection>(mapEntries)
    return {
      ...action,
      GameConfigurations: action.GameConfigurations.merge(newMap)
    }
  })
  .case(Actions.setActiveEmulator, (action, payload) => {
    return {
      ...action,
      ActiveEmulator: payload
    }
  })
  .build()

export default reducer
