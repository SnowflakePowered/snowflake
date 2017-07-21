import * as React from 'react'
import { Route } from 'react-router'
import withSnowflake from 'decorators/withSnowflake'
import PlatformList from 'components/PlatformList/PlatformList'
import SidebarFrame from 'components/SidebarFrame/SidebarFrame'
import GameList from 'components/GameList/GameList'

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

class Switchboard extends React.Component {
  render () {
    return (
      <div>
        <SidebarFrame>
          <Route path='/platforms/' component={PlatformRendererTest} />
          <Route path='/games/' component={GameRendererTest} />
        </SidebarFrame>
      </div>
    )
  }
}

export default Switchboard
