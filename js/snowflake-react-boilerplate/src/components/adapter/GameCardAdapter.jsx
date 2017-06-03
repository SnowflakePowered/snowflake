import React from 'react'
import GameCard from 'components/presentation/game/GameCard'

import aspectRatio from 'snowflake/platformsupport/aspectratio'

const GameCardAdapter = ({ game }) => {
  return (
    <GameCard title={game.Title} {...aspectRatio[game.PlatformId]} publisher={game.Metadata.game_publisher}/>
  )
}

export default GameCardAdapter
