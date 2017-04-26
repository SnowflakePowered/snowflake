import React from 'react'
import PropTypes from 'prop-types'

import injectSheet from 'mui-jss-inject'


import ImageCard from 'components/presentation/ImageCard'
import GameDisplay from 'components/presentation/info/GameDisplay'
import GamePlayButton from 'components/presentation/GamePlayButton'


const styles = {
  container: {
    display: 'flex',
    flexDirection: 'row'
  },
  gameCover: {
    padding: 10
  }
}

const GameDetailsHeaderView = ({classes, gameTitle, gamePublisher, gameCover}) => (
  <div className={classes.container}>
    <div className={classes.gameCover}>
      <ImageCard image={gameCover}/>
    </div>
    <GameDisplay title={gameTitle} publisher={gamePublisher}/>
    <GamePlayButton/>
  </div>
)

GameDetailsHeaderView.propTypes = {
  gameCover: PropTypes.string.isRequired
}
export default injectSheet(styles)(GameDetailsHeaderView)
