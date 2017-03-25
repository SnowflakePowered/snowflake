import React from 'react'

const GameDisplay = ({ games }) => {
  console.log(games)
  return (
    <div>
      {games.map(g =>
        <div>{g.Title}</div>
      )
      }
    </div>
  )
}

export default GameDisplay