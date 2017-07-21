import * as React from 'react'
import * as PropTypes from 'prop-types'

import injectSheet from 'mui-jss-inject'

import GameDisplay from 'components/GameDisplay/GameDisplay'
import GamePlayButton from 'components/GamePlayButton/GamePlayButton'

import { styles } from './GameLaunchHeader.style'

type GameLaunchHeaderProps = {
  classes?: any,
  gameTitle: string,
  gamePublisher: string,
  gameDescription?: string,
  onPlayButtonClicked?: (event: Event) => void
}

const GameLaunchHeader: React.SFC<GameLaunchHeaderProps> = ({ classes, gameTitle, gamePublisher, gameDescription, onPlayButtonClicked }) => (
  <div className={classes.detailContainer}>
    <div className={classes.controlContainer}>
      <div className={classes.topControls}>
        <div className={classes.gameDisplay}>
          <GameDisplay
            title={gameTitle}
            publisher={gamePublisher}
          />
        </div>
        <GamePlayButton onClick={onPlayButtonClicked}/>
      </div>
      <div className={classes.bottomControls}>
        <div className={classes.emulatorSelector}/>
      </div>
    </div>
  </div>
)

GameLaunchHeader.propTypes = {
  gameTitle: PropTypes.string.isRequired,
  gamePublisher: PropTypes.string,
  onPlayButtonClicked: PropTypes.func
}
export default injectSheet<GameLaunchHeaderProps>(styles)(GameLaunchHeader)
