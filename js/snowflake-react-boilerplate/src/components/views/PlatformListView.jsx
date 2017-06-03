import React from 'react'
import injectSheet from 'mui-jss-inject'

import List, { ListItem, ListItemIcon, ListItemText } from 'material-ui/List';
import Typography from 'material-ui/Typography'

const styles = {
  platformSelector: {
    overflowY: 'auto',
    overflowX: 'hidden',
    gridColumn: 'platformSelector'
  },
  container: {
    display: 'grid',
    gridTemplateColumns: '[platformSelector] 50% [info] 50%',
    height: '100%'
  }
}


const PlatformListView = ({ classes, platforms, currentPlatform, onPlatformChanged }) => {

  const handlePlatformChanged = (p) => {
    if (onPlatformChanged) onPlatformChanged(p)
  }

  return (
    <div className={classes.container}>
      <div className={classes.platformSelector}>
        <List>
          {Object.values(platforms).map((p) => 
          <ListItem button onClick={() => handlePlatformChanged(p)}>
            <Typography>{p.FriendlyName}</Typography>
          </ListItem>)}
        </List>
      </div>
    </div>
  )
}

export default injectSheet(styles)(PlatformListView)