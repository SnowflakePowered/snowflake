import { Platform, Game, ConfigurationCollection } from 'snowflake-remoting'
import { RouterState } from 'react-router-redux'

interface State extends RouterState {
  Games: Game[]
  Platforms: Map<string, Platform>,
  ActivePlatform: string,
  ActiveGame: string,
  ActiveGameConfiguration?: ConfigurationCollection
}

export const InitialState: State = {
  Games: [],
  Platforms: new Map<string, Platform>(),
  ActivePlatform: '',
  ActiveGame: '',
  location: null
}

export default State
