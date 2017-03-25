import React, { Component } from 'react'
import UIFrame from './components/uiframe'
import './App.css'
import withSnowflake from './snowflake/Snowflake'
import SnowflakeProvider from './snowflake/SnowflakeProvider'
import MuiThemeProvider from 'material-ui/styles/MuiThemeProvider'
import injectTapEventPlugin from 'react-tap-event-plugin'
injectTapEventPlugin()

import {
  BrowserRouter as Router,
  Route
} from 'react-router-dom'

class App extends Component {
  constructor () {
    super()
    this.UIFrame = withSnowflake(UIFrame)
  }

  render () {
    return (
      <MuiThemeProvider>
        <Router>
          <Route component={(({location}) =>
            <SnowflakeProvider location={location}>
              <this.UIFrame/>
            </SnowflakeProvider>
          )} />
        </Router>
      </MuiThemeProvider>
    )
  }
}

export default App
