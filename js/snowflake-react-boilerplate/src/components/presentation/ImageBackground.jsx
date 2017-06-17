import React from 'react'
import injectSheet from 'mui-jss-inject'

const styles = {
  headerBackgroundContainer: {
    width: '100%',
    maxHeight: '100%',
    position: 'fixed',
    zIndex: '-100000000'
  },
  headerBackground: {
    minWidth: '110%',
    minHeight: '100%',
    filter: 'blur(5px)',
    position: 'fixed',
    zIndex: '-1',
    top: '-50%',
    left: '-5%'
  }
}

const ImageBackground = ({ classes, image }) => (
  <div className={classes.headerBackgroundContainer}>
    <img className={classes.headerBackground}
      src={image} />
  </div>
)

export default injectSheet(styles)(ImageBackground)
