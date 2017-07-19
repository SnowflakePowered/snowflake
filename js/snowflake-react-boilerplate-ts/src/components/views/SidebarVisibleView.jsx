import React from 'react'
import injectSheet from 'mui-jss-inject'

import Sidebar from 'components/views/Sidebar'
const styles = {
  container: {
    width: '100%',
    height: '100%',
    maxHeight: '100%',
    maxWidth: '100%',
    display: 'grid',
    gridTemplateColumns: '64px auto',
    gridTemplateRows: '',
    gridTemplateAreas: [
      '"sidebar main"'
    ]
  },
  sidebarContainer: {
    gridArea: 'sidebar'
  },
  mainContainer: {
    gridArea: 'main',
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
