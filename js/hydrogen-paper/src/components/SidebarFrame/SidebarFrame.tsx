import * as React from 'react'
import injectSheet from 'support/InjectSheet'

import Sidebar from 'components/Sidebar/Sidebar'
import { styles } from './SidebarFrame.style'

const SidebarFrame: React.SFC<{classes?: any}> = ({ classes, children }) => (
  <div className={classes.container}>
    <div className={classes.sidebarContainer}>
      <Sidebar />
    </div>
    <div className={classes.mainContainer}>
        { children }
    </div>
  </div>
)

export default injectSheet(styles)(SidebarFrame)
