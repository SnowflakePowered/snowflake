import * as React from 'react'
import SnowflakeProvider from 'state/SnowflakeProvider'
import { MuiThemeProvider } from 'material-ui/styles'
// import GameCard from 'components/GameCard/GameCard'
import Switchboard from './Switchboard'

class App extends React.Component<{}, {}> {
  render () {
    return (
      <MuiThemeProvider>
          <SnowflakeProvider>
            <div>
              <Switchboard/>
            </div>
          </SnowflakeProvider>
      </MuiThemeProvider>
    )
  }
}

export default App
