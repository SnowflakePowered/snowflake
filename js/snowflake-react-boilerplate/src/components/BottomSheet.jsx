import React from 'react'
import Paper from 'material-ui/Paper'

import injectSheet from 'mui-jss-inject'

const styles = {
  container: {
    width: '100%',
    margin: 10
  },
  paper: {
    width: '100%'
  }
}

const BottomSheet = ({children, classes}) => (
  <div className={classes.container}>
    {children}
  </div>
)

export default injectSheet(styles)(BottomSheet)
