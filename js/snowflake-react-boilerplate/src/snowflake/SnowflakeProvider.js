import React from 'react'
import ReduxState from './ReduxState'
import PropTypes from 'prop-types'

class SnowflakeProvider extends React.Component {
  static childContextTypes = {
    snowflake: PropTypes.object
  }

  getChildContext () {
    return {
      snowflake: {
        platforms: this.props.platforms,
        games: this.props.games,
        state: this.props.state,
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

export default ReduxState(SnowflakeProvider)
