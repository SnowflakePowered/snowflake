import React from 'react'
import withGames from '../../snowflake/withGames'

const GameDisplay = ({ games, filter }) => {
  return (
    <div>
      {games.filter(filter).map(g =>
        <div>{g.Title}</div>
      )
      }
    </div>
  )
}

export default withGames(GameDisplay)
