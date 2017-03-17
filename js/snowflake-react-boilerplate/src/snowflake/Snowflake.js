import React from 'react'
import getDisplayName from 'recompose/getDisplayName'

const withSnowflake = WrappedComponent => {
  return class extends React.Component {
    static contextTypes = {
      snowflake: React.PropTypes.object
    }
    static displayName = `Snowflake(${getDisplayName(WrappedComponent)})`

    render () {
      return <WrappedComponent {...this.props} snowflake={this.context.snowflake}/>
    }
  }
}

export default withSnowflake
