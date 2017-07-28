import { createSelector } from 'reselect'
import State from 'state/State'
import { RouterState } from 'react-router-redux'
import * as queryString from 'query-string'
import QueryList from 'support/QueryList'
import { ConfigurationKey } from 'support/ConfigurationKey'

const activePlatformIdSelector = (state: State) => state.ActivePlatform
const activeGameUuidSelector = (state: State) => state.ActiveGame
const activeEmulatorSelector = (state: State) => state.ActiveEmulator
const activeProfileSelector = (state: State) => state.ActiveGameConfigProfile
const locationSelector = (state: RouterState) => state.location
const elementLoadingStateSelector = (state: State) => state.ElementLoadingStates

export const gameConfigsSelector = (state: State) => state.GameConfigurations
export const gamesSelector = (state: State) => state.Games
export const platformsSelector = (state: State) => state.Platforms

export const queryParamsSelector = createSelector(
  locationSelector,
  location => QueryList(queryString.parse(location!.search) as QueryList)
)

export const activePlatformSelector = createSelector(
  platformsSelector,
  activePlatformIdSelector,
  (platforms, activePlatform) => platforms[activePlatform]
)

export const activeGameSelector = createSelector(
  gamesSelector,
  activeGameUuidSelector,
  (games, gameUuid) => games[gameUuid]
)

export const activePlatformGamesSelector = createSelector(
  gamesSelector,
  activePlatformSelector,
  (games, platform) => Object.values(games).filter(g => platform ? g.PlatformID === platform.PlatformID : 0)
)

export const activeEmulatorConfigurationSelector = createSelector(
  activeGameUuidSelector,
  activeEmulatorSelector,
  activeProfileSelector,
  gameConfigsSelector,
  (gameGuid, emulatorName, profileName, configs) => {
    const key = ConfigurationKey(gameGuid, emulatorName, profileName)
    const config = configs.get(key)
    return { key: key, config: config }
  }
)

export const isElementLoadingSelector = createSelector(
  elementLoadingStateSelector,
  (elementLoadingState) => (elementId: string) => elementLoadingState.get(elementId) || false
)
