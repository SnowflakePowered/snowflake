import React, { Component } from 'react'
import UIFrame from './components/uiframe/'
import './App.css'
import withSnowflake from './snowflake/withSnowflake/'
import SnowflakeProvider from './snowflake/SnowflakeProvider/'
import MuiThemeProvider from 'material-ui/styles/MuiThemeProvider'
import injectTapEventPlugin from 'react-tap-event-plugin'
injectTapEventPlugin()

import {
  BrowserRouter as Router,
  Route
} from 'react-router-dom'

import { ConnectedRouter, routerReducer, routerMiddleware, push } from 'react-router-redux'


class App extends Component {
  render () {
    return (
      <MuiThemeProvider>
        <ConnectedRouter history={this.props.history}>
          <Route component={(({location}) =>
            <SnowflakeProvider location={location}>
              <UIFrame/>
            </SnowflakeProvider>
          )} />
        </ConnectedRouter>
      </MuiThemeProvider>
    )
  }
}

export default App
