import * as React from 'react'
import { Route } from 'react-router'
import withSnowflake from 'decorators/withSnowflake'
import PlatformList from 'components/PlatformList/PlatformList'
import SidebarFrame from 'components/SidebarFrame/SidebarFrame'

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
          <Route path='/platforms/' component={PlatformRendererTest} />
        </SidebarFrame>
      </div>
    )
  }
}

export default Switchboard
