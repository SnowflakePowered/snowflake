import React from 'react'
import { Route } from 'react-router-dom'
import { ConnectedRouter } from 'react-router-redux'
import injectSheet from 'mui-jss-inject'

import MuiStyleManager from './MuiStyleManager'
import { blue, pink } from 'material-ui/styles/colors'

import GameCard from 'components/presentation/GameCard'
import SnowflakeProvider from 'snowflake/SnowflakeProvider'

const muiStyle = MuiStyleManager({ blue, pink })

const App = ({ history, classes }) => {
  return (
    <muiStyle>
      <ConnectedRouter history={history}>
        <Route component={(({ location }) =>
          <SnowflakeProvider location={location}>
              <GameCard image="https://upload.wikimedia.org/wikipedia/en/d/db/NewSuperMarioBrothers.jpg"
        title="Square New Super Mario Bros. Chou Nagai Monji Gaiden NEW" publisher="Nintendo" square/>
          </SnowflakeProvider>
        )} />
      </ConnectedRouter>
    </muiStyle>
  )
}

export default App
