import { Platform, Game } from 'snowflake-remoting'

interface State {
  Games: Game[]
  Platforms: Map<string, Platform>,
  ActivePlatform: string,
  ActiveGame: string
}

export const InitialState: State = {
  Games: [],
  Platforms: new Map<string, Platform>(),
  ActivePlatform: '',
  ActiveGame: ''
}

export default State
