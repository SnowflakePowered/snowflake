import React from 'react'
import GameCard from 'components/presentation/GameCard'

import aspectRatio from 'snowflake/platformsupport/aspectratio'

const GameCardAdapter = ({ game }) => {
  console.log (game)
  console.log(aspectRatio["NINTENDO_SNE"])
  return (
    <GameCard title={game.Title} {...aspectRatio[game.PlatformId]} publisher={game.Metadata.game_publisher}/>
  )
}

export default GameCardAdapter
