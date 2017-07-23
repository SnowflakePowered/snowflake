import actionCreatorFactory from 'typescript-fsa'
import { Platform, Game, ConfigurationCollection } from 'snowflake-remoting'

const actionCreator = actionCreatorFactory()

type SNOWFLAKE_REFRESH_GAMES = 'SNOWFLAKE_REFRESH_GAMES'
const SNOWFLAKE_REFRESH_GAMES: SNOWFLAKE_REFRESH_GAMES = 'SNOWFLAKE_REFRESH_GAMES'

type SNOWFLAKE_REFRESH_PLATFORMS = 'SNOWFLAKE_REFRESH_PLATFORMS'
const SNOWFLAKE_REFRESH_PLATFORMS: SNOWFLAKE_REFRESH_PLATFORMS = 'SNOWFLAKE_REFRESH_PLATFORMS'

type STATE_SET_ACTIVE_PLATFORM = 'STATE_SET_ACTIVE_PLATFORM'
const STATE_SET_ACTIVE_PLATFORM: STATE_SET_ACTIVE_PLATFORM = 'STATE_SET_ACTIVE_PLATFORM'

type STATE_SET_ACTIVE_GAME = 'STATE_SET_ACTIVE_GAME'
const STATE_SET_ACTIVE_GAME: STATE_SET_ACTIVE_GAME = 'STATE_SET_ACTIVE_GAME'

type SNOWFLAKE_REFRESH_ACTIVE_GAME_CONFIGURATION = 'SNOWFLAKE_REFRESH_ACTIVE_GAME_CONFIGURATION'
const SNOWFLAKE_REFRESH_ACTIVE_GAME_CONFIGURATION: SNOWFLAKE_REFRESH_ACTIVE_GAME_CONFIGURATION = 'SNOWFLAKE_REFRESH_ACTIVE_GAME_CONFIGURATION'

type SNOWFLAKE_RETRIEVE_GAME_CONFIGURATION = 'SNOWFLAKE_RETRIEVE_GAME_CONFIGURATION'
const SNOWFLAKE_RETRIEVE_GAME_CONFIGURATION: SNOWFLAKE_RETRIEVE_GAME_CONFIGURATION = 'SNOWFLAKE_RETRIEVE_GAME_CONFIGURATION'

type LOCATION_CHANGE = '@@router/LOCATION_CHANGE'
const LOCATION_CHANGE = '@@router/LOCATION_CHANGE'

export const refreshPlatforms = actionCreator
      .async<void, { [platformId: string]: Platform }>(SNOWFLAKE_REFRESH_PLATFORMS)

export const refreshGames = actionCreator
       .async<void, { [gameGuid: string]: Game }>(SNOWFLAKE_REFRESH_GAMES)

export const refreshActiveGameConfiguration = actionCreator
       .async<{emulatorName: string, gameUuid: string}, ConfigurationCollection>(SNOWFLAKE_REFRESH_ACTIVE_GAME_CONFIGURATION)

export const retrieveGameConfiguration = actionCreator
      .async<{gameGuid: string, profileName: string}, { [emulatorName: string]: ConfigurationCollection }>(SNOWFLAKE_RETRIEVE_GAME_CONFIGURATION)

export const setActivePlatform = actionCreator<string>(STATE_SET_ACTIVE_PLATFORM)

export const setActiveGame = actionCreator<string>(STATE_SET_ACTIVE_GAME)

export const locationChange = actionCreator<any>(LOCATION_CHANGE)
