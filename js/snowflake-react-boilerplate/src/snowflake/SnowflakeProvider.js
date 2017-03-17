import React from 'react'
import State from './State'

class SnowflakeContext extends React.Component {
  static childContextTypes = {
    snowflake: React.PropTypes.object
  }

  getChildContext () {
    return {
      snowflake: {
        stone: this.props.stone,
        games: this.props.games,
        ui: this.props.ui,
        store: {
          actions: this.props.actions,
          dispatch: this.props.dispatch
        }
      }
    }
  }

  render () {
    return this.props.children
  }
}

export default State(SnowflakeContext)
