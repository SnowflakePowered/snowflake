import React from 'react'
import queryString from 'query-string'
import Immutable from 'seamless-immutable'
import { withRouter } from 'react-router'
import compose from 'recompose/compose'
import withPlatforms from 'snowflake/compose/withPlatforms'

const withQueryState = (WrappedComponent) => {
  return compose(withRouter, withPlatforms)(class extends React.Component {

    buildQueryStateObject (queryState) {
      let output = {
        platform: undefined
      }
      if (queryState.platform && this.props.platforms) {
        output = {...output, platform: this.props.platforms.get(queryState.platform)}
      }
      return Immutable(output)
    }

    render () {
      const queryState = queryString.parse(this.props.location.search)
      const queryStateObject = this.buildQueryStateObject(queryState)
      console.log(this.props)
      return <WrappedComponent {...this.props} queryState={queryStateObject} queryParams={queryState}/>
    }
  })
}

export default withQueryState
