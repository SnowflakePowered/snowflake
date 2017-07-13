import React from 'react'
import GameCard from 'components/presentation/game/GameCard'

import aspectRatio from 'snowflake/platformsupport/aspectratio'
import { get } from 'lodash'
const GameCardAdapter = ({ game }) => {
  return (
    <GameCard title={game.Title} {...aspectRatio[game.PlatformID]} publisher={get(game.Metadata.game_publisher, "Value", "")} guid={game.Guid} platformID={game.PlatformID}/>
  )
}

export default GameCardAdapter
