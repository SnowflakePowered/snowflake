import React from 'react'
import { Route } from 'react-router-dom'
import { ConnectedRouter } from 'react-router-redux'
import injectSheet from 'mui-jss-inject'
import GameListViewAdapter from 'components/adapter/GameListViewAdapter'
import SidebarVisibleView from 'components/views/SidebarVisibleView'
import SnowflakeProvider from 'snowflake/SnowflakeProvider'

const styles = {

}

const App = ({ history, classes }) => {
  return (
    <ConnectedRouter history={history}>
      <Route render={() =>
        <SnowflakeProvider>
          <SidebarVisibleView>
            <GameListViewAdapter/>
          </SidebarVisibleView>
        </SnowflakeProvider>
      } />
    </ConnectedRouter>
  )
}

export default injectSheet(styles)(App)
