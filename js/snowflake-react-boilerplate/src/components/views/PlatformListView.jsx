import React from 'react'
import injectSheet from 'mui-jss-inject'

import SidebarVisibleView from 'components/views/SidebarVisibleView'
import List, { ListItem } from 'material-ui/List'
import Typography from 'material-ui/Typography'
import BottomSheet from 'components/BottomSheet'

import PlatformDisplayAdapter from 'components/adapter/PlatformDisplayAdapter'

import { grey } from 'material-ui/styles/colors'

import Link from 'components/Link'

const styles = {
  platformSelector: {
    overflowY: 'auto',
    overflowX: 'hidden',
    gridColumn: 'platformSelector'
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
    gridTemplateColumns: '[platformInfo] 80% [platformMenu] 20%'
  },
  menu: {
    gridColumn: 'platformMenu'
  },
  platformInfoDisplay: {
    gridColumn: 'platformInfo'
  }
}

const PlatformListView = ({ classes, platforms, currentPlatform }) => {
  console.log(Array.from(platforms))
  console.log(Object.values(platforms.entries()))
  return (
    <div className={classes.container}>
      <div className={classes.platformSelector}>
        <List>
          {Array.from(platforms).map(([k, p]) =>
            <Link to={`/platforms/${p.PlatformID}`}>
              <ListItem key={p.PlatformID} button>
                <Typography>{p.FriendlyName}</Typography>
              </ListItem>
            </Link>)}
        </List>
      </div>
      <div className={classes.platformDisplay}>
        <div className={classes.platformImage}>
          <Typography>Placeholder</Typography>
        </div>
        <BottomSheet className={classes.platformInformation}>
          <div className={classes.platformInformationInner}>
            <div className={classes.platformInfoDisplay}>
              <PlatformDisplayAdapter platform={currentPlatform} />
            </div>
            <div className={classes.menu}>
              {/* todo: refactor out this ugliness */}
            </div>
          </div>
        </BottomSheet>
      </div>
    </div>
  )
}

export default injectSheet(styles)(PlatformListView)
