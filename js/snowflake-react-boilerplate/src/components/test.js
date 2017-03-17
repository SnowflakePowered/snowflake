import React, { Component } from 'react'
import RaisedButton from 'material-ui/RaisedButton';

class Test extends Component {
  render () {
    console.log(this.context)
    return (
      <div className="Tester">
        <RaisedButton label="Hello World!"/>
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
