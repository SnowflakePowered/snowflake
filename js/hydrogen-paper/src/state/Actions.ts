import actionCreatorFactory from 'typescript-fsa'
import { Platform, Game } from 'snowflake-remoting/dist/snowflake'

const actionCreator = actionCreatorFactory()

type SNOWFLAKE_REFRESH_GAMES = 'SNOWFLAKE_REFRESH_GAMES'
const SNOWFLAKE_REFRESH_GAMES: SNOWFLAKE_REFRESH_GAMES = 'SNOWFLAKE_REFRESH_GAMES'

type SNOWFLAKE_REFRESH_PLATFORMS = 'SNOWFLAKE_REFRESH_PLATFORMS'
const SNOWFLAKE_REFRESH_PLATFORMS: SNOWFLAKE_REFRESH_PLATFORMS = 'SNOWFLAKE_REFRESH_PLATFORMS'

type STATE_SET_ACTIVE_PLATFORM = 'STATE_SET_ACTIVE_PLATFORM'
const STATE_SET_ACTIVE_PLATFORM: STATE_SET_ACTIVE_PLATFORM = 'STATE_SET_ACTIVE_PLATFORM'

type STATE_SET_ACTIVE_GAME = 'STATE_SET_ACTIVE_GAME'
const STATE_SET_ACTIVE_GAME: STATE_SET_ACTIVE_GAME = 'STATE_SET_ACTIVE_GAME'

const LOCATION_CHANGE = '@@router/LOCATION_CHANGE'

export const refreshPlatforms = actionCreator
      .async<void, Map<string, Platform>>(SNOWFLAKE_REFRESH_PLATFORMS)

export const refreshGames = actionCreator
       .async<void, Game[]>(SNOWFLAKE_REFRESH_GAMES)

export const setActivePlatform = actionCreator<string>(STATE_SET_ACTIVE_PLATFORM)

export const setActiveGame = actionCreator<string>(STATE_SET_ACTIVE_GAME)

export const locationChange = actionCreator<any>(LOCATION_CHANGE)
