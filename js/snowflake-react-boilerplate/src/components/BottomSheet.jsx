import React from 'react'
import Paper from 'material-ui/Paper'

import injectSheet from 'mui-jss-inject'

const styles = {
  container: {
    height: '100%',
    margin: 10
  }
}

const BottomSheet = ({children, classes}) => (
  <Paper>
    <div className={classes.container}>
      {children}
    </div>
  </Paper>
)

export default injectSheet(styles)(BottomSheet)
