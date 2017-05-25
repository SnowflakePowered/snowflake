import React from 'react'
import { Route } from 'react-router-dom'
import { ConnectedRouter } from 'react-router-redux'
import injectSheet from 'mui-jss-inject'
import GameListView from 'components/views/GameListView'

import SnowflakeProvider from 'snowflake/SnowflakeProvider'

const styles = {

}

const App = ({ history, classes }) => {
  return (
    <ConnectedRouter history={history}>
      <Route render={() =>
          <SnowflakeProvider>
            <GameListView/>
          </SnowflakeProvider>
      } />
    </ConnectedRouter>
  )
}

export default injectSheet(styles)(App)
