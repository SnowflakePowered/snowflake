import React from 'react'
import GameLaunchHeaderView from 'components/views/GameLaunchHeaderView'
import injectSheet from 'mui-jss-inject'

import { title, publisher, description } from 'stories/utils/constants'

const styles = {
  container: {
    height: '100%',
    width: '100%'
  }
}

const GameDetailsHeaderViewStory = ({classes}) => (
  <div className={classes.container}>
    <GameLaunchHeaderView
      gameTitle = {title}
      gamePublisher = {publisher}
      gameDescription = {description}
    />
  </div>
)

export default injectSheet(styles)(GameDetailsHeaderViewStory)
