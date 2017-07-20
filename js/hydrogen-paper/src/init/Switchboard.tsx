import * as React from 'react'
import { Route } from 'react-router'
import withSnowflake from 'decorators/withSnowflake'
import PlatformList from 'components/PlatformList/PlatformList'
import SidebarFrame from 'components/SidebarFrame/SidebarFrame'
import MapRouteToState from 'routing/MapRouteToState'

const PlatformRendererTest = withSnowflake(({ snowflake }) =>
  (
    <PlatformList
      platforms={snowflake.Platforms}
      platform={snowflake.ActivePlatform}
      gameCount={snowflake.ActivePlatformGames.length}
    />
  )
)

class Switchboard extends React.Component {
  render () {
    return (
      <div>
        <SidebarFrame>
          <Route path='*' render={MapRouteToState}/>
          <Route path='/platforms/' component={PlatformRendererTest} />
        </SidebarFrame>
      </div>
    )
  }
}

export default Switchboard
