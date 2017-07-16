import React, { Component } from 'react'
import AppBar from 'material-ui/AppBar'
import './index.css'

class AppHeader extends Component {
  render () {
    return (
      <AppBar className="ui-toolbar" title="Snowflake" />
    )
  }
}

export default AppHeader
