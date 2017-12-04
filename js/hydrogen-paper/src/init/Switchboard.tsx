import * as React from 'react'
import { Route } from 'react-router'
import withSnowflake from 'decorators/withSnowflake'
import PlatformList from 'components/PlatformList/PlatformList'
import SidebarFrame from 'components/SidebarFrame/SidebarFrame'
import GameList from 'components/GameList/GameList'
import GameDetails from 'components/GameDetails/GameDetails'
import { get } from 'lodash'

const PlatformRendererTest = withSnowflake(({ snowflake }) =>
  (
    <PlatformList
      platforms={snowflake!.Platforms}
      platform={snowflake!.ActivePlatform}
      gameCount={snowflake!.ActivePlatformGames.length}
    />
  )
)

const GameRendererTest = withSnowflake(({ snowflake }) =>
    (
      <GameList games={snowflake!.ActivePlatformGames}
        platform={snowflake!.ActivePlatform}/>
    )
  )

const GameViewRenderTest = withSnowflake(({snowflake}) => (
  <GameDetails game={snowflake!.ActiveGame}
    platform={snowflake!.ActivePlatform}
    gameTitle={snowflake!.ActiveGame ? snowflake!.ActiveGame.Title : 'Unknown'}
    gamePublisher={get(snowflake!.ActiveGame, 'Metadata.game_publisher.Value', 'Unknown Publisher')}
    gameDescription={get(snowflake!.ActiveGame, 'Metadata.game_description.Value', 'No Description Found')}/>
))

class Switchboard extends React.Component {
  render () {
    return (
      <div>
        <SidebarFrame>
          <Route path='/platforms/' component={PlatformRendererTest} />
          <Route path='/games/' component={GameRendererTest} />
          <Route path='/gamedetail/' component={GameViewRenderTest}/>
        </SidebarFrame>
      </div>
    )
  }
}

export default Switchboard
