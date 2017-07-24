import * as React from 'react'
import injectSheet from 'support/InjectSheet'
import * as _ from 'lodash'
import List, { ListItem } from 'material-ui/List'
import Typography from 'material-ui/Typography'
import BottomSheet from 'components/BottomSheet/BottomSheet'

import Button from 'material-ui/Button'
import PlatformDisplayContainer from 'containers/PlatformDisplay/PlatformDisplayContainer'
import Link from 'components/Link/Link'

import { styles } from './PlatformList.style'
import { Platform } from 'snowflake-remoting'

type PlatformListProps = {
  classes?: any,
  platforms: { [platformId: string]: Platform },
  platform: Platform,
  gameCount: number
}
const PlatformList: React.SFC<PlatformListProps> = ({ classes, platforms, platform, gameCount }) => {
  return (
    <div className={classes.container}>
      <div className={classes.platformSelector}>
        <List>
          {Object.entries(platforms).map(([k, p]) =>
            <Link to={`?platform=${p.PlatformID}`} key={p.PlatformID}>
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
              <PlatformDisplayContainer platform={platform}
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

export default injectSheet(styles)(PlatformList)
