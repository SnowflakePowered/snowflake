import React from 'react'
import injectSheet from 'mui-jss-inject'

import Sidebar from 'components/views/Sidebar'
import { grey } from 'material-ui/styles/colors'

const styles = {
  container: {
    width: '100%',
    height: '100%',
    display: 'grid',
    gridTemplateColumns: '[sidebar] 64px [main] auto',
    gridTemplateRows: ''
  },
  sidebarContainer: {
    gridColumnArea: 'sidebar'
  },
  mainContainer: {
    gridColumnStart: 'main',
    height: '100vh'
  }
}

const SidebarVisibleView = ({ classes, children }) => (
  <div className={classes.container}>
    <div className={classes.sidebarContainer}>
      <Sidebar />
    </div>
    <div className={classes.mainContainer}>
        { children }
    </div>
  </div>
)

export default injectSheet(styles)(SidebarVisibleView)
