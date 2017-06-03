import React from 'react'
import injectSheet from 'mui-jss-inject'

import SidebarVisibleView from 'components/views/SidebarVisibleView'
import List, { ListItem } from 'material-ui/List'
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

 // todo refactor out to presentation and action launcher
  const handlePlatformChanged = (p) => {
    if (onPlatformChanged) onPlatformChanged(p)
  }

  return (
    <SidebarVisibleView>
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
    </SidebarVisibleView>
  )
}

export default injectSheet(styles)(PlatformListView)