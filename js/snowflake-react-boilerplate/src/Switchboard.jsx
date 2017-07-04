import React from 'react'
import { Route, Link } from 'react-router-dom'
import withSnowflake from 'snowflake/compose/withSnowflake'
import withPlatforms from 'snowflake/compose/withPlatforms'
import compose from 'recompose/compose'
import _ from 'lodash'
import SidebarVisibleView from 'components/views/SidebarVisibleView'
import PlatformListView from 'components/views/PlatformListView'
import withQueryState from 'snowflake/compose/withQueryState'
import withGames from 'snowflake/compose/withGames'
import GameDisplayView from 'components/views/GameDisplayView'
import GameListView from 'components/views/GameListView'

const PlatformRendererTest = compose(withPlatforms, withQueryState)(({platforms, queryState}) => (
  <PlatformListView platforms={platforms} platform={queryState.platform}/>
))

const GameRendererTest = compose(withGames, withQueryState)(({games, queryState}) => (
  <GameListView games={games} platform={queryState.platform}/>
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
          <Route path="/platforms/" component={PlatformRendererTest}/>
          <Route path="/games/" component={GameRendererTest}/>
        </SidebarVisibleView>
      </div>
    )
  }
}
export default withSnowflake(Switchboard)
