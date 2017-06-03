import React from 'react'
import PropTypes from 'prop-types'
import wrapDisplayName from 'recompose/wrapDisplayName'

const withState = (WrappedComponent) => {
  return class extends React.Component {
    static contextTypes = {
      snowflake: PropTypes.object
    }

    static displayName = wrapDisplayName(WrappedComponent, 'Games')

    render () {
      return <WrappedComponent {...this.props} state={this.context.snowflake.state}/>
    }
  }
}

export default withState
