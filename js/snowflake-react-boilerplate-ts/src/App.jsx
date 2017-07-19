import React from 'react'
import { BrowserRouter } from 'react-router-dom'
import injectSheet from 'mui-jss-inject'
import SnowflakeProvider from 'snowflake/SnowflakeProvider'

import Switchboard from './Switchboard'

const styles = {

}

const App = ({ history, classes }) => {
  return (
    <BrowserRouter>
      <SnowflakeProvider>
        <Switchboard/>
      </SnowflakeProvider>
    </BrowserRouter>
  )
}

export default injectSheet(styles)(App)
