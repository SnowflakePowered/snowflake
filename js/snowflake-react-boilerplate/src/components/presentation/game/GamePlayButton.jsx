import React from 'react'
import PlayArrow from 'material-ui-icons/PlayArrow'
import IconButton from 'material-ui/IconButton'
import { CircularProgress } from 'material-ui/Progress'

import injectSheet from 'mui-jss-inject'
import grey from 'material-ui/colors/grey'
import common from 'material-ui/colors/common'
const styles = {
  button: {
    borderRadius: '50%',
    width: '100%',
    height: '100%',
    padding: 8,
    display: 'inline-flex',
    justifyContent: 'center',
    boxSizing: 'border-box',
    overflow: 'hidden',
    backgroundColor: common.white
  },
  arrow: {
    color: grey[800],
    height: 24,
    width: 24
  },
  buttonContainer: {
    height: 40,
    width: 40,
    display: 'block',
    position: 'relative',
    zIndex: 100
  },
  pulse: {
    '$buttonContainer:hover &': {
      transform: 'scale(1.15)'
    },
    position: 'absolute',
    top: 0,
    bottom: 0,
    width: '100%',
    height: '100%',
    borderRadius: '50%',
    background: 'rgba(0,0,0,0.34)',
    zIndex: -1,
    transform: 'scale(1.05)',
    transition: 'transform 0.2s ease'
  },
  progress: {
    color: grey[500],
    position: 'absolute',
    top: 0,
    left: 0,
    zIndex: 10
  }
}

const GamePlayButton = ({classes, onClick, loading}) => (
  <div className={classes.buttonContainer}>
    <div className={classes.pulse}/>
    {loading ? <CircularProgress size={40} className={classes.progress} /> : ''}
    <IconButton className={classes.button} onClick={onClick}>
      <PlayArrow className={classes.arrow}/>
    </IconButton>
  </div>
)

export default injectSheet(styles)(GamePlayButton)
