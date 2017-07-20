import * as React from 'react'
import './App.css'
import SnowflakeProvider from 'state/SnowflakeProvider'
import { MuiThemeProvider } from 'material-ui/styles'
// import GameCard from 'components/GameCard/GameCard'
import { BrowserRouter } from 'react-router-dom'
import MapRouteToState from 'routing/MapRouteToState'
import ImageCard from 'components/ImageCard/ImageCard'

class App extends React.Component<{}, {}> {
  render () {
    return (
      <MuiThemeProvider>
        <BrowserRouter>
          <SnowflakeProvider>
            <MapRouteToState/>
            <div>
              <ImageCard image='hello' elevation={1}/>
            </div>
          </SnowflakeProvider>
        </BrowserRouter>
      </MuiThemeProvider>
    )
  }
}

export default App
