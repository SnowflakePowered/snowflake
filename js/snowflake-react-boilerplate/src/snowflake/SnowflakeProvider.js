import React from 'react'
import State from './State'
import PropTypes from 'prop-types'

class SnowflakeProvider extends React.Component {
  static childContextTypes = {
    snowflake: PropTypes.object
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

export default State(SnowflakeProvider)
