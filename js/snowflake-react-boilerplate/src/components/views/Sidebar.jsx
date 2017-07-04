import React from 'react'
import injectSheet from 'mui-jss-inject'

import List, {
  ListItem
} from 'material-ui/List'

import MenuIcon from 'material-ui-icons/Menu'
import GamepadIcon from 'material-ui-icons/VideogameAsset'
import { white, grey } from 'material-ui/styles/colors'
import Link from 'components/Link'

const styles = {
  container: {
    height: '100%',
    width: 64,
    backgroundColor: white
  },
  icon: {
    height: 32,
    width: 32,
    color: grey[700]
  }
}

const Sidebar = ({ classes }) => (
  <div className={classes.container}>
    <List>
      <ListItem button>
        <MenuIcon className={classes.icon} />
      </ListItem>
      <Link to="/platforms">
        <ListItem button>
          <GamepadIcon className={classes.icon} />
        </ListItem>
      </Link>
    </List>
  </div>
)

export default injectSheet(styles)(Sidebar)
