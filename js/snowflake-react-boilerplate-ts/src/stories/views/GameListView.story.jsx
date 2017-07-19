import React from 'react'
import GameListView from 'components/views/GameListView'
import state from 'stories/state'
const GameListViewStory = () => (
  <GameListView games={state.games}/>
)

export default GameListViewStory
