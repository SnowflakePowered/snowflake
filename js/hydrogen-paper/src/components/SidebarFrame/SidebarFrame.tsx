import * as React from 'react'
import injectSheet, { StyleProps } from 'support/InjectSheet'
import { NoProps } from 'support/NoProps'
import Sidebar from 'components/Sidebar/Sidebar'
import { styles } from './SidebarFrame.style'

const SidebarFrame: React.SFC<NoProps & StyleProps> = ({ classes, children }) => (
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
