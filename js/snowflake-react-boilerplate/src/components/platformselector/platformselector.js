import React from 'react'
import PlatformDisplay from './platformdisplay'
import { List, ListItem } from 'material-ui/List'

const PlatformSelector = ({ snowflake }) => {
  return (
    <List>
      {
        [...snowflake.stone.platforms.values()].map(p => {
          return (
            <ListItem onTouchTap={() => snowflake.store.actions.ui.setActivePlatform(p)}>
              <PlatformDisplay platform={p} />
            </ListItem>
          )
        })
      }
    </List>
  )
}

export default PlatformSelector
