import React from 'react'
import injectSheet from 'react-jss'
import GameDisplay from 'components/presentation/info/GameDisplay'

import { red500 } from 'material-ui/styles/colors'

export const GameBlack = () => (
  <GameDisplay
    gameTitle="Super Mario World"
    region="US"
    publisher="Nintendo"
    year="1990" />
)

export const GameWhiteWithBackground = injectSheet({
  container: {
    color: 'white',
    backgroundColor: red500,
    height: '100%'
  },
  infoDisplay: {
    height: '100%'
  }
})(({ classes }) => (
  <div className={classes.container}>
    <GameDisplay
      className={classes.infoDisplay}
      gameTitle="Super Mario World"
      region="US"
      publisher="Nintendo"
      year="1990" />
  </div>
));
