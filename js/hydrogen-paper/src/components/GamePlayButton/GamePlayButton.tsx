import * as React from 'react'
import PlayArrow from 'material-ui-icons/PlayArrow'
import IconButton from 'material-ui/IconButton'
import { CircularProgress } from 'material-ui/Progress'

import injectSheet, { StyleProps } from 'support/InjectSheet'
import { styles } from './GamePlayButton.style'

type GamePlayButtonProps = {
  onClick?: (event: Event) => void,
  loading?: boolean
}
const GamePlayButton: React.SFC<GamePlayButtonProps & StyleProps> = ({classes, onClick, loading}) => (
  <div className={classes.buttonContainer}>
    <div className={classes.pulse}/>
    {loading ? <CircularProgress size={40} className={classes.progress} /> : ''}
    <IconButton className={classes.button} onClick={onClick}>
      <PlayArrow className={classes.arrow}/>
    </IconButton>
  </div>
)

export default injectSheet(styles)(GamePlayButton)
