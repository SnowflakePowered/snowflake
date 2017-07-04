import React from 'react'
import { withRouter } from 'react-router'
import { Route, Link } from 'react-router-dom'
import withSnowflake from 'snowflake/compose/withSnowflake'
import withPlatforms from 'snowflake/compose/withPlatforms'
import compose from 'recompose/compose'
import _ from 'lodash'
import SidebarVisibleView from 'components/views/SidebarVisibleView'
import PlatformListView from 'components/views/PlatformListView'


const getMatchPlatform = (match, platforms) => platforms.get(match.params.platformId)

const withMatchPlatform = (WrappedComponent) => {
  return withPlatforms(class extends React.Component {
    render () {
      console.log(this.props.match)
      const matchPlatform = getMatchPlatform(this.props.match, this.props.platforms)
      return <WrappedComponent {...this.props} matchPlatform={matchPlatform}/>
    }
  })
}

const PlatformRendererTest = withMatchPlatform(({platforms, matchPlatform}) => (
  <PlatformListView platforms={platforms} currentPlatform={matchPlatform}/>
))

class Switchboard extends React.Component {
  componentDidMount () {
    this.props.snowflake.store.actions.platforms.beginRefreshPlatforms()
    this.props.snowflake.store.actions.games.beginRefreshGames()
  }

  render () {
    return (
      <div>
        <SidebarVisibleView>
          <Route path="/platforms/:platformId" component={PlatformRendererTest}/>
          <Route path="/platforms/" component={PlatformRendererTest}/>
        </SidebarVisibleView>
      </div>
    )
  }
}
export default withSnowflake(Switchboard)
