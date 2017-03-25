import React, { Component } from 'react'
import UIFrame from './components/uiframe'
import Test from './components/test'
import withSnowflake from './snowflake/Snowflake'
import SnowflakeProvider from './snowflake/SnowflakeProvider'
import MuiThemeProvider from 'material-ui/styles/MuiThemeProvider'
import injectTapEventPlugin from 'react-tap-event-plugin'
import './App.css'

injectTapEventPlugin()

import {
  BrowserRouter as Router,
  Route,
  Redirect
} from 'react-router-dom'

class App extends Component {
  constructor () {
    super()
    this.PlatformUI = withSnowflake(Test)
  }

  render () {
    // const platforms = () => <this.PlatformUI games={this.props.games}/>
// <this.UIFrame/>
    return (
      <MuiThemeProvider>
        <Router>
          <Route component={({location}) =>
            <SnowflakeProvider location={location}>
              <UIFrame className="ui-frame"/>
            </SnowflakeProvider>
          } />
        </Router>
      </MuiThemeProvider>
    )
  }
}

export default App
