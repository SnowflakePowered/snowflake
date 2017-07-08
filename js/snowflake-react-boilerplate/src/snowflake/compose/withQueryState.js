import React from 'react'
import queryString from 'query-string'
import Immutable from 'seamless-immutable'
import { withRouter } from 'react-router'
import compose from 'recompose/compose'
import withPlatforms from 'snowflake/compose/withPlatforms'
import withGames from 'snowflake/compose/withGames'

const withQueryState = (WrappedComponent) => {
  return compose(withRouter, withPlatforms, withGames)(class extends React.Component {
    buildQueryStateObject (queryState) {
      let output = {
        platform: undefined,
        platformGames: undefined,
        game: undefined
      }
      if (queryState.platform && this.props.platforms) {
        output = {...output,
          platform: this.props.platforms.get(queryState.platform),
          platformGames: this.props.games.filter((g) => {
            if (queryState.platform === undefined) return false
            return g.PlatformID === queryState.platform
            // push this to redux?
          })
        }
      }
      if (queryState.game && this.props.games) {
        output = {...output,
          game: this.props.games.filter(g => g.Guid === queryState.game)[0]
        }
      }

      return Immutable(output)
    }

    render () {
      const queryState = queryString.parse(this.props.location.search)
      const queryStateObject = this.buildQueryStateObject(queryState)
      return <WrappedComponent {...this.props} queryState={queryStateObject} queryParams={queryState}/>
    }
  })
}

export default withQueryState
