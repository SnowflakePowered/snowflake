import actionCreatorFactory from 'typescript-fsa'
import {
  Platform,
  Game,
  ConfigurationCollection,
  ConfigurationValue
} from 'snowflake-remoting'
import { ConfigurationKey } from 'support/ConfigurationKey'

const actionCreator = actionCreatorFactory()

enum SnowflakeActions {
  REFRESH_GAMES = '@@snowflake/REFRESH_GAMES',
  REFRESH_PLATFORMS = '@@snowflake/REFRESH_PLATFORMS',
  RETRIEVE_GAME_CONFIGURATION = '@@snowflake/RETRIEVE_GAME_CONFIGURATION',
  REFRESH_GAME_CONFIGURATION = '@@snowflake/REFRESH_GAME_CONFIGURATION',
  REFRESH_MANY_GAME_CONFIGURATIONS = '@@snowflake/REFRESH_MANY_GAME_CONFIGURATIONS'
}

enum StateActions {
  SET_ACTIVE_GAME = '@@state/SET_ACTIVE_GAME',
  SET_ACTIVE_PLATFORM = '@@state/SET_ACTIVE_PLATFORM',
  SET_ACTIVE_EMULATOR = '@@state/SET_ACTIVE_EMULATOR',
  SET_ACTIVE_CONFIGURATION_PROFILE = '@@state/SET_ACTIVE_CONFIGURATION_PROFILE',
  SET_ELEMENT_LOADING_STATE = '@@state/SET_ELEMENT_LOADING_STATE'
}

type LOCATION_CHANGE = '@@router/LOCATION_CHANGE'
const LOCATION_CHANGE: LOCATION_CHANGE = '@@router/LOCATION_CHANGE'

export const refreshPlatforms = actionCreator
      .async<void, { [platformId: string]: Platform }>(SnowflakeActions.REFRESH_PLATFORMS)

export const refreshGames = actionCreator
       .async<void, { [gameGuid: string]: Game }>(SnowflakeActions.REFRESH_GAMES)

export const retrieveGameConfiguration = actionCreator
      .async<{gameGuid: string, profileName: string}, { [emulatorName: string]: ConfigurationCollection }>(SnowflakeActions.RETRIEVE_GAME_CONFIGURATION)

export const refreshGameConfiguration = actionCreator
      .async<{configKey: ConfigurationKey, newValue: ConfigurationValue}, ConfigurationCollection>(SnowflakeActions.REFRESH_GAME_CONFIGURATION)

export const refreshGameConfigurations = actionCreator
      .async<{configKey: ConfigurationKey, newValues: ConfigurationValue[]}, ConfigurationCollection>(SnowflakeActions.REFRESH_MANY_GAME_CONFIGURATIONS)

export const setActivePlatform = actionCreator<string>(StateActions.SET_ACTIVE_PLATFORM)

export const setActiveGame = actionCreator<string>(StateActions.SET_ACTIVE_GAME)

export const setActiveEmulator = actionCreator<string>(StateActions.SET_ACTIVE_EMULATOR)

export const setActiveConfigurationProfile = actionCreator<string>(StateActions.SET_ACTIVE_CONFIGURATION_PROFILE)

export const locationChange = actionCreator<any>(LOCATION_CHANGE)

export const setElementLoadingState = actionCreator<{elementId: string, loadingState: boolean}>(StateActions.SET_ELEMENT_LOADING_STATE)
