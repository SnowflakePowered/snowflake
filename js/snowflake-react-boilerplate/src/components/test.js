import React, { Component } from 'react'


class Test extends Component {
  render () {
    return (
      <div className="Tester">
        <div className="Platforms">
            <ul>
              { ([...this.props.platforms.values()].map(p => <li key={p.PlatformID}>{ p.FriendlyName }</li>)) }
            </ul>
        </div>
      </div>
    )
  }
}

export default Test
