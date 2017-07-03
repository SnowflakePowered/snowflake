import React from 'react'
import injectSheet from 'mui-jss-inject'

import SidebarVisibleView from 'components/views/SidebarVisibleView'
import List, { ListItem } from 'material-ui/List'
import Typography from 'material-ui/Typography'
import BottomSheet from 'components/BottomSheet'

import Button from 'material-ui/Button'

import PlatformDisplayAdapter from 'components/adapter/PlatformDisplayAdapter'

import { grey } from 'material-ui/styles/colors'

const styles = {
  platformSelector: {
    overflowY: 'auto',
    overflowX: 'hidden',
    gridColumn: 'platformSelector',
    zIndex: 2
  },
  container: {
    display: 'grid',
    gridTemplateColumns: '[platformSelector] 30% [info] auto',
    height: '100%'
  },
  platformDisplay: {
    display: 'grid',
    gridTemplateRows: '[platformImage] 60% [platformInfo] auto'
  },
  platformImage: {
    backgroundColor: grey[200]
  },
  platformInformation: {
    gridRow: 'platformInfo'
  },
  platformInformationInner: {
    display: 'grid',
    gridTemplateRows: '[platformInfo] 80% [platformMenu] 20%',
    height: '100%',
    width: '100%'
  },
  menu: {
    gridRow: 'platformMenu',
    display: 'flex',
    alignItems: 'baseline',
    justifyContent: 'flex-end'

  },
  platformInfoDisplay: {
    gridRow: 'platformInfo'
  }
}

const PlatformListView = ({ classes, platforms, currentPlatform, onPlatformChanged }) => {
  // todo refactor out to presentation and action launcher
  const handlePlatformChanged = (p) => {
    if (onPlatformChanged) onPlatformChanged(p)
  }

  return (
    <div className={classes.container}>
      <div className={classes.platformSelector}>
        <List>
          {Object.values(platforms).map(p =>
            <ListItem key={p.PlatformID} button onClick={() => handlePlatformChanged(p)}>
              <Typography>{p.FriendlyName}</Typography>
            </ListItem>)}
        </List>
      </div>
      <div className={classes.platformDisplay}>
        <div className={classes.platformImage}>
          <Typography>Placeholder</Typography>
        </div>
        <BottomSheet className={classes.platformInformation}>
          <div className={classes.platformInformationInner}>
            <div className={classes.platformInfoDisplay}>
              <PlatformDisplayAdapter platform={platforms[currentPlatform]} />
            </div>
            <div className={classes.menu}>
              {/* todo: refactor out this ugliness */}
              <Button>Games</Button>
              <Button>Controllers</Button>
            </div>
          </div>
        </BottomSheet>
      </div>
    </div>
  )
}

export default injectSheet(styles)(PlatformListView)
