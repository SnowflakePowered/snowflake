import React from 'react'
import { Route } from 'react-router-dom'
import { ConnectedRouter } from 'react-router-redux'
import injectSheet from 'react-jss'

// Material-UI Start
import MuiThemeProvider from 'material-ui/styles/MuiThemeProvider'
import injectTapEventPlugin from 'react-tap-event-plugin'

// Snowflake Start
import SnowflakeProvider from './snowflake/SnowflakeProvider'
import GameCard from './components/presentation/GameCard'

injectTapEventPlugin()

const styles = {
}

const App = ({ history, classes }) => {
  return (
    <ConnectedRouter history={history}>
      <Route component={(({ location }) =>
        <SnowflakeProvider location={location}>
          <MuiThemeProvider>
            <div className={classes.Test}><GameCard /></div>
          </MuiThemeProvider>
        </SnowflakeProvider>
      )} />
    </ConnectedRouter>
  )
}

export default injectSheet(styles)(App)
