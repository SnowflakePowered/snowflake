import * as React from 'react'
import injectSheet from 'mui-jss-inject'

import List, {
  ListItem
} from 'material-ui/List'

import MenuIcon from 'material-ui-icons/Menu'
import GamepadIcon from 'material-ui-icons/VideogameAsset'
import Link from 'components/Link/Link'
import { styles } from './Sidebar.style'

const Sidebar: React.SFC<{classes?: any}> = ({ classes }) => (
  <div className={classes.container}>
    <List>
      <Link to={`/games?platform=`}>
        <ListItem button>
          <MenuIcon className={classes.icon} />
        </ListItem>
      </Link>
      <Link to={`/platforms?platform=`}>
        <ListItem button>
          <GamepadIcon className={classes.icon} />
        </ListItem>
      </Link>
    </List>
  </div>
)

export default injectSheet(styles)(Sidebar)
