import React from 'react'
import GameDetailsHeaderView from 'components/views/GameDetailsHeaderView'

import { title, portraitCover, publisher } from 'stories/utils/constants'
export const GameDetailsHeaderViewStory = () => (
  <GameDetailsHeaderView
    gameTitle = {title}
    gameCover = {portraitCover}
    gamePublisher = {publisher}
  />
)

export default GameDetailsHeaderViewStory

