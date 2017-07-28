import { Platform, Game, ConfigurationCollection } from 'snowflake-remoting'
import { RouterState } from 'react-router-redux'
import * as Immutable from 'immutable'
import { ConfigurationKey } from 'support/ConfigurationKey'

interface State extends RouterState {
  Games: { [gameGuid: string]: Game }
  Platforms: { [platformId: string]: Platform }
  ActivePlatform: string,
  ActiveGame: string,
  ActiveEmulator: string,
  ActiveGameConfigProfile: string,
  GameConfigurations: Immutable.Map<ConfigurationKey, ConfigurationCollection>
  ElementLoadingStates: Immutable.Map<string, boolean>
}

export const InitialState: State = {
  Games: {},
  Platforms: {},
  ActivePlatform: '',
  ActiveGame: '',
  location: null,
  ActiveGameConfigProfile: '',
  ActiveEmulator: '',
  GameConfigurations: Immutable.Map(),
  ElementLoadingStates: Immutable.Map()
}

export default State
