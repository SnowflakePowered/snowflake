import * as React from 'react'
import './App.css'
import SnowflakeProvider from 'state/SnowflakeProvider'
import { MuiThemeProvider } from 'material-ui/styles'
// import GameCard from 'components/GameCard/GameCard'
import { BrowserRouter } from 'react-router-dom'
import MapRouteToState from 'routing/MapRouteToState'

class App extends React.Component<any, any> {
  render () {
    return (
      <MuiThemeProvider>
        <BrowserRouter>
          <SnowflakeProvider>
            <MapRouteToState/>
            <div>
              Hello World
            </div>
          </SnowflakeProvider>
        </BrowserRouter>
      </MuiThemeProvider>
    )
  }
}

export default App
