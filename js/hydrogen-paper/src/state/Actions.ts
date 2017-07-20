import actionCreatorFactory from 'typescript-fsa'
import { Platform, Game } from 'snowflake-remoting/dist/snowflake'

const actionCreator = actionCreatorFactory()

type SNOWFLAKE_REFRESH_GAMES = 'SNOWFLAKE_REFRESH_GAMES'
const SNOWFLAKE_REFRESH_GAMES: SNOWFLAKE_REFRESH_GAMES = 'SNOWFLAKE_REFRESH_GAMES'

type SNOWFLAKE_REFRESH_PLATFORMS = 'SNOWFLAKE_REFRESH_PLATFORMS'
const SNOWFLAKE_REFRESH_PLATFORMS: SNOWFLAKE_REFRESH_PLATFORMS = 'SNOWFLAKE_REFRESH_PLATFORMS'

type STATE_SET_ACTIVE_PLATFORM = 'STATE_SET_ACTIVE_PLATFORM'
const STATE_SET_ACTIVE_PLATFORM: STATE_SET_ACTIVE_PLATFORM = 'STATE_SET_ACTIVE_PLATFORM'

export const refreshPlatforms = actionCreator
      .async<void, Map<string, Platform>>(SNOWFLAKE_REFRESH_PLATFORMS)

export const refreshGames = actionCreator
       .async<void, Game[]>(SNOWFLAKE_REFRESH_GAMES)

export const setCurrentPlatform = actionCreator<Platform>(STATE_SET_ACTIVE_PLATFORM)
