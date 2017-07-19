import React from 'react'
import injectSheet from 'mui-jss-inject'
import _ from 'lodash'
import List, { ListItem } from 'material-ui/List'
import Typography from 'material-ui/Typography'
import BottomSheet from 'components/BottomSheet'

import Button from 'material-ui/Button'

import PlatformDisplayAdapter from 'components/adapter/PlatformDisplayAdapter'

import grey from 'material-ui/colors/grey'
import Link from 'components/Link'

const styles = {
  platformSelector: {
    overflowY: 'auto',
    overflowX: 'hidden',
    gridArea: 'selector',
    zIndex: 2
  },
  container: {
    display: 'grid',
    gridTemplateColumns: '30% auto',
    gridTemplateAreas: [
      '"selector info"'
    ],
    height: '100%'
  },
  platformDisplay: {
    display: 'grid',
    gridTemplateRows: '60% auto',
    gridTemplateAreas: '"image" "info"'
  },
  platformImage: {
    backgroundColor: grey[200],
    gridArea: 'image'
  },
  platformInformation: {
    gridArea: 'info'
  },
  platformInformationInner: {
    display: 'grid',
    gridTemplateRows: '80% 20%',
    gridTemplateAreas: '"info" "menu"',
    height: '100%',
    width: '100%'
  },
  menu: {
    gridArea: 'menu',
    display: 'flex',
    alignItems: 'center',
    justifyContent: 'flex-end'

  },
  platformInfoDisplay: {
    gridArea: 'info'
  }
}

const PlatformListView = ({ classes, platforms, platform, gameCount }) => {
  console.log(Array.from(platforms))
  console.log(Object.values(platforms.entries()))
  return (
    <div className={classes.container}>
      <div className={classes.platformSelector}>
        <List>
          {Array.from(platforms).map(([k, p]) =>
            <Link to={`?platform=${p.PlatformID}`}>
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
              <PlatformDisplayAdapter platform={platform}
                gameCount={gameCount}/>
            </div>
            <div className={classes.menu}>
              {/* todo: refactor out this ugliness */}
              <Link to={`/games?platform=${_.get(platform, 'PlatformID', '')}`}>
                <Button>Games</Button>
              </Link>
              <Button>Controllers</Button>
            </div>
          </div>
        </BottomSheet>
      </div>
    </div>
  )
}

export default injectSheet(styles)(PlatformListView)
