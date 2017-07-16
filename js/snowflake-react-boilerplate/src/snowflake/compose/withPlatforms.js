import React from 'react'
import PropTypes from 'prop-types'
import wrapDisplayName from 'recompose/wrapDisplayName'

const withPlatforms = (WrappedComponent) => {
  return class extends React.Component {
    static contextTypes = {
      snowflake: PropTypes.object
    }

    static displayName = wrapDisplayName(WrappedComponent, 'Platforms')

    render () {
      return <WrappedComponent {...this.props} 
        platforms={this.context.snowflake.platforms}/>
    }
  }
}

export default withPlatforms
