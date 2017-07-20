import { createSelector } from 'reselect'
import State from 'state/State'

const activePlatformIdSelector = (state: State) => state.ActivePlatform
const activeGameUuidSelector = (state: State) => state.ActiveGame

export const gamesSelector = (state: State) => state.Games
export const platformsSelector = (state: State) => state.Platforms

export const activePlatformSelector = createSelector(
  platformsSelector,
  activePlatformIdSelector,
  (platforms, activePlatform) => platforms.get(activePlatform)
)

export const activeGameSelector = createSelector(
  gamesSelector,
  activeGameUuidSelector,
  (games, gameUuid) => games.filter(game => game.Guid === gameUuid)[0]
)

export const activePlatformGamesSelector = createSelector(
  gamesSelector,
  activePlatformSelector,
  (games, platform) => games.filter(g => platform ? g.PlatformID === platform.PlatformID : 0)
)
