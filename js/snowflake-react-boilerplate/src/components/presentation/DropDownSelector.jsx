import React, { Component } from 'react'
import List, { ListItem, ListItemText } from 'material-ui/List'
import Menu, { MenuItem } from 'material-ui/Menu'
import injectSheet from 'mui-jss-inject'

const styleSheet = theme => ({
  root: {
    width: '100%',
    maxWidth: '360px',
    background: theme.palette.background.paper
  }
})

const options = [
  'Nestopia',
  'Mesen',
  'Higan'
]

class SimpleListMenu extends Component {
  state = {
    anchorEl: undefined,
    open: false,
    selectedIndex: 1
  };

  button = undefined;

  handleClickListItem = event => this.setState({ open: true, anchorEl: event.currentTarget });

  handleMenuItemClick = (event, index) => this.setState({ selectedIndex: index, open: false });

  handleRequestClose = () => this.setState({ open: false });

  render () {
    return (
      <div className={this.props.classes.root}>
        <List>
          <ListItem
            button
            onClick={this.handleClickListItem}
          >
            <ListItemText
              primary={options[this.state.selectedIndex]}
              secondary="For mockup purposes only, waiting for proper SelectField support."
            />
          </ListItem>
        </List>
        <Menu
          id="lock-menu"
          anchorEl={this.state.anchorEl}
          className={this.props.classes.menu}
          open={this.state.open}
          onRequestClose={this.handleRequestClose}
        >
          {options.map((option, index) => {
            return (
              <MenuItem
                key={option}
                selected={index === this.state.selectedIndex}
                onClick={event => this.handleMenuItemClick(event, index)}
              >
                {option}
              </MenuItem>
            )
          })}
        </Menu>
      </div>
    )
  }
}

export default injectSheet(styleSheet)(SimpleListMenu)
