import * as React from 'react'
import injectSheet from 'mui-jss-inject'

import Sidebar from 'components/Sidebar/Sidebar'
import { styles } from './SidebarFrame.style'

const SidebarVisibleView: React.SFC<{classes?: any}> = ({ classes, children }) => (
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
