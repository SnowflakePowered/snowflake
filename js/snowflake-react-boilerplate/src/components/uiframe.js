import React, { Component } from 'react'
import _ from 'lodash'
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
        <AppHeader className="ui-header" />
        <div className="ui-main-child" style={{ paddingLeft: '200px' }}>
          This is where the UI Frame goes!!
              {this.props.children}
        </div>
        <div>{_.get(this.props.snowflake.ui.activePlatform, 'PlatformID', 'NO_PLATFORM')}</div> { /* todo: replace null proagation */}
        <this.PlatformSelector />
      </div>
    )
  }
}

export default UIFrame
