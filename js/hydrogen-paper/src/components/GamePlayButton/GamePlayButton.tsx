import * as React from 'react'
import PlayArrow from 'material-ui-icons/PlayArrow'
import IconButton from 'material-ui/IconButton'
import { CircularProgress } from 'material-ui/Progress'

import injectSheet from 'mui-jss-inject'
import { styles } from './GamePlayButton.style'

type GamePlayButtonProps = {
  classes?: any,
  onClick?: (event: Event) => void,
  loading?: boolean
}
const GamePlayButton: React.SFC<GamePlayButtonProps> = ({classes, onClick, loading}) => (
  <div className={classes.buttonContainer}>
    <div className={classes.pulse}/>
    {loading ? <CircularProgress size={40} className={classes.progress} /> : ''}
    <IconButton className={classes.button} onClick={onClick}>
      <PlayArrow className={classes.arrow}/>
    </IconButton>
  </div>
)

export default injectSheet<GamePlayButtonProps>(styles)(GamePlayButton)
