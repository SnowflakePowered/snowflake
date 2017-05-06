import React from 'react'
import injectSheet from 'mui-jss-inject'


const styles = {
  container: {

  }
}
const GameGridView = ({classes, children}) => (
  <div className={classes.container}>
    {children}
  </div>
)

export default injectSheet(styles)(GameGridView)