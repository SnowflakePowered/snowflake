import React from 'react'
import PropTypes from 'prop-types'
import wrapDisplayName from 'recompose/wrapDisplayName'

const withGames = (WrappedComponent) => {
  return class extends React.Component {
    static contextTypes = {
      snowflake: PropTypes.object
    }

    static displayName = wrapDisplayName(WrappedComponent, 'Games')

    render () {
      return <WrappedComponent {...this.props} games={this.context.snowflake.games}/>
    }
  }
}

export default withGames
