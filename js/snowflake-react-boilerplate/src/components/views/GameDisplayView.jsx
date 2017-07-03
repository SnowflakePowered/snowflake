import React from 'react'
import injectSheet from 'mui-jss-inject'
import ImageBackground from 'components/presentation/ImageBackground'
import GameLaunchHeaderView from 'components/views/GameLaunchHeaderView'

import Paper from 'material-ui/Paper'
import ImageCard from 'components/presentation/ImageCard'

const style = {
  container: {
    display: 'flex',
    overflowX: 'hidden',
    justifyContent: 'center'
  },
  centeredContainer: {
    width: '90%',
    display: 'grid',
    gridTemplateRows: '[gameHeader] 300px [main] auto',
    paddingBottom: 10
  },
  gameHeader: {
    //background: 'white',
    gridRow: 'gameHeader',
    display: 'flex',
    alignItems: 'center',
    background: 'rgba(255,255,255,0.5)',
    margin: [10, 0],
    borderRadius: 2
  },
  imageContainer: {
    maxHeight: 250,
    maxWidth: 250,
    height: 250,
    width: 250,
    display: 'flex',
    justifyContent: 'center',
    alignContent: 'center',
    paddingLeft: 20,
    paddingRight: 20
  },
  gameMain: {
    overflowY: 'auto'
  },
  mainPaper: {
    height: 1000
  }
}

/*

        </div>
        */

const GameDisplayView = ({ classes }) => (
  <div className={classes.container}>
    <ImageBackground image="https://r.mprd.se/media/images/35787-Super_Mario_World_(USA)-2.jpg" />
    <div className={classes.centeredContainer}>
      <div className={classes.gameHeader}>
        <div className={classes.imageContainer}>
          <ImageCard
            image="http://vignette2.wikia.nocookie.net/mario/images/6/60/SMBBoxart.png/revision/latest?cb=20120609143443"
            // image="https://upload.wikimedia.org/wikipedia/en/3/32/Super_Mario_World_Coverart.png"
          />
        </div>
        <div className={classes.gameTitle}>
          <GameLaunchHeaderView
            gameTitle="TITLE"
            gamePublisher="PUBLISHER"
          />
        </div>
      </div>

      <Paper className={classes.mainPaper}>
        Hello World
      </Paper>
    </div>
  </div>
)

export default injectSheet(style)(GameDisplayView)
