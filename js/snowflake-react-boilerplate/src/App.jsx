import React from 'react'
import { Route } from 'react-router-dom'
import { ConnectedRouter } from 'react-router-redux'
import injectSheet from 'mui-jss-inject'
import GameCard from 'components/presentation/GameCard'

import SnowflakeProvider from 'snowflake/SnowflakeProvider'

const styles = {

}

const App = ({ history, classes }) => {
  return (
    <ConnectedRouter history={history}>
      <Route render={() =>
          <SnowflakeProvider>
            <GameCard image="https://upload.wikimedia.org/wikipedia/en/d/db/NewSuperMarioBrothers.jpg"
                title="Square New Super Mario Bros. Chou Nagai Monji Gaiden NEW" publisher="Nintendo" square />
          </SnowflakeProvider>
      } />
    </ConnectedRouter>
  )
}

export default injectSheet(styles)(App)
