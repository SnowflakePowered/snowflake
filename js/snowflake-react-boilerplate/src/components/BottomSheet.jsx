import React from 'react'
import Paper from 'material-ui/Paper'

import injectSheet from 'mui-jss-inject'

const styles = {
  container: {
    width: '100%',
    margin: 10
  },
  paper: {
    width: '110%',
    zIndex: -1
  }
}

const BottomSheet = ({children, classes}) => (
  <Paper className={classes.paper}>
    <div className={classes.container}>
      {children}
    </div>
  </Paper>
)

export default injectSheet(styles)(BottomSheet)
