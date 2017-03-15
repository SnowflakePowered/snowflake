import React, { Component } from 'react'
import { Link } from 'react-router-dom'

class UIFrame extends Component {
  render () {
    return (
        <div className="ui-frame">
            This is where the UI Frame goes!!
            <Link to="/platforms">View Platforms</Link>
            <div className="ui-main-child" style={{paddingLeft: '200px'}}>
                { this.props.children }
            </div>
        </div>
    )
  }
}

export default UIFrame
