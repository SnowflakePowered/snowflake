import * as React from 'react'
import GameCard from 'components/GameCard/GameCard'

import { Game } from 'snowflake-remoting'
import { get } from 'lodash'

type GameCardContainerProps = {
  game: Game
}

export const ASPECT_RATIOS = {
  PORTRAIT: { portrait: true, landscape: false, square: false },
  LANDSCAPE: { portrait: false, landscape: true, square: false },
  SQUARE: { portrait: false, landscape: false, square: true }
}

const platformAspectMapping = {
  NINTENDO_NES: ASPECT_RATIOS.PORTRAIT,
  NINTENDO_SNES: ASPECT_RATIOS.LANDSCAPE
}

const GameCardContainer: React.SFC<GameCardContainerProps> = ({ game }) => {
  return (
    <GameCard title={game.Title} { ...platformAspectMapping[game.PlatformID] }
      publisher={get(game.Metadata.game_publisher, 'Value', '')} guid={game.Guid} platformID={game.PlatformID}/>
  )
}

export default GameCardContainer
