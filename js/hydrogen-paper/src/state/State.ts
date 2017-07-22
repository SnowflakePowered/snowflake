import { Platform, Game, ConfigurationCollection } from 'snowflake-remoting'
import { RouterState } from 'react-router-redux'

interface State extends RouterState {
  Games: { [gameGuid: string]: Game }
  Platforms: { [platformId: string]: Platform }
  ActivePlatform: string,
  ActiveGame: string,
  ActiveGameConfiguration?: ConfigurationCollection
}

export const InitialState: State = {
  Games: {},
  Platforms: {},
  ActivePlatform: '',
  ActiveGame: '',
  location: null
}

export default State
