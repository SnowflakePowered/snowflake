import { Platform, Game, ConfigurationCollection } from 'snowflake-remoting'
import { RouterState } from 'react-router-redux'
import * as Immutable from 'immutable'
import { ConfigurationKey } from 'support/ConfigurationKey'

interface State extends RouterState {
  Games: { [gameGuid: string]: Game }
  Platforms: { [platformId: string]: Platform }
  ActivePlatform: string,
  ActiveGame: string,
  GameConfigurations: Immutable.Map<ConfigurationKey, ConfigurationCollection>
}

export const InitialState: State = {
  Games: {},
  Platforms: {},
  ActivePlatform: '',
  ActiveGame: '',
  location: null,
  GameConfigurations: Immutable.Map()
}

export default State
