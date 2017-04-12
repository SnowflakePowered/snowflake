import React from 'react'
import PlatformDisplay from '../platformdisplay'
import { ListItem } from 'material-ui/List'
import Divider from 'material-ui/Divider'
import Paper from 'material-ui/Paper'

import './index.css'

const PlatformBar = ({ platform }) => {
  return (
    <Paper className="platform-display-bar">
      <ListItem className="current">
        {(platform !== undefined ? <PlatformDisplay platform={platform}/> : 'No Platform Selected')}
      </ListItem>
      <Divider/>
    </Paper>
  )
}

export default PlatformBar
