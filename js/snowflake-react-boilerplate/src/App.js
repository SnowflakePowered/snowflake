import React, { Component } from 'react'
import SnowflakeProvider from './snowflake/SnowflakeProvider'
import { Route } from 'react-router-dom'
import { ConnectedRouter } from 'react-router-redux'

class App extends Component {
  render () {
    return (
      <ConnectedRouter history={this.props.history}>
        <Route component={(({location}) =>
          <SnowflakeProvider location={location}>
            <div>Hello World</div>
          </SnowflakeProvider>
          )} />
      </ConnectedRouter>
    )
  }
}

export default App
