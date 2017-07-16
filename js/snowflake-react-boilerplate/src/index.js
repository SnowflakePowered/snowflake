import React from 'react'
import ReactDOM from 'react-dom'
import App from './App'
import { Provider } from 'react-redux'
import Store from './store'
import createHistory from 'history/createBrowserHistory'

import MuiStyleManager from './MuiStyleManager'
import { MuiThemeProvider } from 'material-ui/styles'
import blue from 'material-ui/colors/blue'
import pink from 'material-ui/colors/pink'
require('map.prototype.tojson')
const history = createHistory()
const StoreInstance = Store({
  platforms: new Map(),
  games: []
}, history)

ReactDOM.render(
  <MuiThemeProvider>
    <Provider store={StoreInstance}>
      <App history={history}/>
    </Provider>
  </MuiThemeProvider>,
 document.getElementById('root')
)
