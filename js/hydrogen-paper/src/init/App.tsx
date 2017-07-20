import * as React from 'react'
import './App.css'
import SnowflakeProvider from 'decorators/SnowflakeProvider'
import { MuiThemeProvider } from 'material-ui/styles'
// import GameCard from 'components/GameCard/GameCard'
import { BrowserRouter, Route } from 'react-router-dom'
import CurrentPlatformRouter from 'routing/CurrentPlatformRouter'

class App extends React.Component<any, any> {
  render () {
    return (
      <MuiThemeProvider>
        <BrowserRouter>
          <SnowflakeProvider>
            <Route path='*' component={CurrentPlatformRouter}/>
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
