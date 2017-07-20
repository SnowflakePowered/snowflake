import { Platform, Game } from 'snowflake-remoting'

interface RootState {
  Games: Game[]
  Platforms: Map<string, Platform>,
  CurrentPlatform?: Platform
}

export const InitialState: RootState = {
  Games: [],
  Platforms: new Map<string, Platform>(),
  CurrentPlatform: undefined
}

export default RootState
