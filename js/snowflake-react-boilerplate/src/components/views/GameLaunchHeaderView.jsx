import React from 'react'
import PropTypes from 'prop-types'

import injectSheet from 'mui-jss-inject'

import GameDisplay from 'components/presentation/info/GameDisplay'
import GamePlayButton from 'components/presentation/game/GamePlayButton'
import DropDownSelector from 'components/presentation/DropDownSelector'

const styles = {
  container: {
    display: 'grid',
    gridTemplateColumns: 'auto auto',
    margin: 10
  },
  gameCover: {
    padding: '0 10px',
    display: 'flex',
    justifyContent: 'center'
  },
  controlContainer: {
    paddingRight: 10,
    display: 'flex',
    flexDirection: 'column',
    justifyContent: 'space-between'
  },
  bottomControls: {
    display: 'flex',
    flexDirection: 'column'
  },
  topControls: {
    display: 'flex',
    flexDirection: 'row'
  },
  gameDisplay: {
    paddingRight: 10
  },
  emulatorSelector: {
    maxWidth: 240
  }
}

const GameLaunchHeaderView = ({ classes, gameTitle, gamePublisher, gameDescription, onPlayButtonClicked }) => (
  <div className={classes.detailContainer}>
    <div className={classes.controlContainer}>
      <div className={classes.topControls}>
        <div className={classes.gameDisplay}>
          <GameDisplay
            title={gameTitle}
            publisher={gamePublisher}
            description={gameDescription}
          />
        </div>
        <GamePlayButton onClicked={onPlayButtonClicked}/>
      </div>
      <div className={classes.bottomControls}>
        <div className={classes.emulatorSelector}>
          
        </div>
      </div>
    </div>
  </div>
)

GameLaunchHeaderView.propTypes = {
  gameTitle: PropTypes.string.isRequired,
  gamePublisher: PropTypes.string,
  onPlayButtonClicked: PropTypes.func
}
export default injectSheet(styles)(GameLaunchHeaderView)
