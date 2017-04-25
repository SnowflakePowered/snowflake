import React from 'react'
import PropTypes from 'prop-types'
import wrapDisplayName from 'recompose/wrapDisplayName'

const withSnowflake = (WrappedComponent) => {
  return class extends React.Component {
    static contextTypes = {
      snowflake: PropTypes.object
    }

    static displayName = wrapDisplayName(WrappedComponent, 'Snowflake')

    render () {
      return <WrappedComponent {...this.props} snowflake={this.context.snowflake}/>
    }
  }
}

export default withSnowflake
