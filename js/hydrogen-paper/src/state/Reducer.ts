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
    const mapEntries: [ConfigurationKey, ConfigurationCollection][] = Object.entries(configs).map(([emulatorName, config]) => {
      const key: ConfigurationKey = ConfigurationKey(gameGuid, emulatorName, profileName)
      const entry: [ConfigurationKey, ConfigurationCollection] = [Immutable.fromJS(key), config]
      return entry
    })
    const newMap = Map<ConfigurationKey, ConfigurationCollection>(mapEntries)
    return {
      ...action,
      GameConfigurations: action.GameConfigurations.merge(newMap)
    }
  })
  .case(Actions.refreshGameConfiguration.done, (action, payload) => {
    const config = payload.result
    const { gameGuid, profileName, emulatorName } = payload.params.configKey
    const key: ConfigurationKey = ConfigurationKey(gameGuid, emulatorName, profileName)
    const newMap = Map<ConfigurationKey, ConfigurationCollection>([[Immutable.fromJS(key), config]])
    return {
      ...action,
      GameConfigurations: action.GameConfigurations.merge(newMap)
    }
  })
  .case(Actions.refreshGameConfigurations.done, (action, payload) => {
    const config = payload.result
    const { gameGuid, profileName, emulatorName } = payload.params.configKey
    const key: ConfigurationKey = ConfigurationKey(gameGuid, emulatorName, profileName)
    const newMap = Map<ConfigurationKey, ConfigurationCollection>([[Immutable.fromJS(key), config]])
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
  .case(Actions.setActiveConfigurationProfile, (action, payload) => {
    return {
      ...action,
      ActiveGameConfigProfile: payload
    }
  })
  .case(Actions.setElementLoadingState, (action, payload) => {
    return {
      ...action,
      ElementLoadingStates: action.ElementLoadingStates.set(payload.elementId, payload.loadingState)
    }
  })
  .build()

export default reducer
