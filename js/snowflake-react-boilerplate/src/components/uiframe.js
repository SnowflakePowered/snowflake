import React, { Component } from 'react'
import { Route, Link } from 'react-router-dom'
import AppHeader from './appheader'
import PlatformSelector from './platformselector/platformselector'
import withSnowflake from '../snowflake/Snowflake'
import './uiframe.css'

class UIFrame extends Component {
  constructor () {
    super()
    this.PlatformSelector = withSnowflake(PlatformSelector)
  }

  render () {
    return (
        <div className="ui-frame">
            <AppHeader className="ui-header"/>
            <div className="ui-main-child" style={{paddingLeft: '200px'}}>
              This is where the UI Frame goes!!
              { this.props.children }
            </div>
            <div>{(this.props.snowflake.ui.activePlatform !== undefined ? this.props.snowflake.ui.activePlatform.PlatformID : "Nope")}</div>
            <this.PlatformSelector/>
        </div>
    )
  }
}

export default UIFrame
