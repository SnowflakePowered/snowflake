import React, { Component } from 'react'
import { Route, Link, Redirect } from 'react-router-dom'
import AppHeader from './appheader'
import PlatformBar from './platformselector/platformbar'
import PlatformSelector from './platformselector/platformselector'
import withSnowflake from '../snowflake/Snowflake'
import './uiframe.css'
import GameDisplay from './gamedisplay'
//.filter(g => g.PlatformID === location.state.platform.PlatformID)
class UIFrame extends Component {
  constructor() {
    super()
    this.PlatformBar = withSnowflake(PlatformBar)
    this.PlatformSelector = withSnowflake(PlatformSelector)
  }

  render() {
    return (
      <div className="ui-frame">
        <AppHeader className="ui-header" />
        <Route exact path="/" component={() =>
          <Redirect to="/games" />
        } />

        <Route path="/games/:PlatformID" component={({match}) => {
          let platform = this.props.snowflake.stone.platforms.get(match.params.PlatformID)
          return (
            <div className="ui-main-child">
              <Link to="/platforms">
                <this.PlatformBar platform={platform}/>
              </Link>
              <GameDisplay games={this.props.snowflake.games.filter(g => g.PlatformId === platform.PlatformID)}/>
            </div>
          )
        }} />

        <Route path="/platforms" component={() => {
          return (
            <this.PlatformSelector/>
          )
        }} />
      </div>
    )
  }
}

export default withSnowflake(UIFrame)
