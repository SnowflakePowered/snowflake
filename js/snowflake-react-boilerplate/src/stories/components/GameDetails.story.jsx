import React from 'react'
import injectSheet from 'react-jss'
import GameDetails from 'components/presentation/game/GameDetails'

export const GameDetailsStory = injectSheet({
  container: {
    width: '80%'
  },
  infoDisplay: {
    height: '100%'
  }
})(({ classes }) => (
  <div className={classes.container}>
    <GameDetails
      image="http://vignette2.wikia.nocookie.net/mario/images/6/60/SMBBoxart.png/revision/latest?cb=20120609143443"
      title="Super Mario World"
      publisher="Nintendo"
      year="1990"
      summary="Super Mario World,[a] subtitled Super Mario Bros. 4[b] for its original Japanese release"
    />
  </div>)
  )
