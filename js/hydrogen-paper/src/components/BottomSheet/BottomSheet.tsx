import * as React from 'react'
import Paper from 'material-ui/Paper'

import injectSheet from 'mui-jss-inject'

const styles = {
  container: {
    height: '100%',
    padding: 10,
    boxSizing: 'border-box'
  }
}

type BottomSheetProps = {
  classes?: any,
  children: React.ReactNode[] | React.ReactNode,
  className?: string
}

const BottomSheet: React.StatelessComponent<BottomSheetProps> = ({children, classes}) => (
  <Paper>
    <div className={classes.container}>
      { children }
    </div>
  </Paper>
)

export default injectSheet<BottomSheetProps>(styles)(BottomSheet)
