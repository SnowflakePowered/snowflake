import React from 'react'
import wrapDisplayName from 'recompose/wrapDisplayName'

const withPlatforms = (WrappedComponent) => {
  return class extends React.Component {
    static contextTypes = {
      snowflake: React.PropTypes.object
    }

    static displayName = wrapDisplayName(WrappedComponent, 'Platforms')

    render () {
      return <WrappedComponent {...this.props} platforms={this.context.snowflake.stone.platforms}/>
    }
  }
}

export default withPlatforms
