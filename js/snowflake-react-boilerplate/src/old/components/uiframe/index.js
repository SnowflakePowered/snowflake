import React, { Component } from 'react'
import { Route, Link, Redirect } from 'react-router-dom'
import AppHeader from '../appheader'
import PlatformBar from '../platformbar'
import PlatformSelector from '../platformselector'
import withSnowflake from '../../snowflake/withSnowflake'
import GameDisplay from '../gamedisplay'

import './index.css'

// .filter(g => g.PlatformID === location.state.platform.PlatformID)
class UIFrame extends Component {
  render () {
    return (
      <div className="ui-frame">
        <AppHeader className="ui-header" />
        <Route exact path="/" component={() =>
          <Redirect to="/platforms" />
        } />

        <Route path="/games/:PlatformID" component={({match}) => {
          const platform = this.props.snowflake.stone.platforms.get(match.params.PlatformID)
          return (
            <div className="ui-main-child">
              <Link to="/platforms">
                <PlatformBar platform={platform}/>
              </Link>
              <GameDisplay filter={(g => g.PlatformId === platform.PlatformID)}/>
            </div>
          )
        }} />

        <Route path="/platforms" component={() => {
          return (
            <PlatformSelector/>
          )
        }} />
      </div>
    )
  }
}

export default withSnowflake(UIFrame)
