import * as React from 'react'
import injectSheet, { StyleProps } from "support/InjectSheet";
import withSnowflake from 'decorators/withSnowflake'

import List, {
  ListItem
} from 'material-ui/List'

import MenuIcon from 'material-ui-icons/Menu'
import GamepadIcon from 'material-ui-icons/VideogameAsset'
import Link from 'components/Link/Link'
import { styles } from './Sidebar.style'

const Sidebar = withSnowflake<StyleProps>(({ classes, snowflake }) => (
  <div className={classes.container}>
    <List>
      <Link to={`/games?platform=${snowflake!.ActivePlatform ? snowflake!.ActivePlatform.PlatformID : ''}`}>
        <ListItem button>
          <MenuIcon className={classes.icon} />
        </ListItem>
      </Link>
      <Link to={`/platforms?platform=${snowflake!.ActivePlatform ? snowflake!.ActivePlatform.PlatformID : ''}`}>
        <ListItem button>
          <GamepadIcon className={classes.icon} />
        </ListItem>
      </Link>
    </List>
  </div>
))

export default injectSheet(styles)(Sidebar)
