import React from 'react'
import ReactDOM from 'react-dom'
import App from './App'
import { Provider } from 'react-redux'
import Store from './store'
import createHistory from 'history/createBrowserHistory'


require('map.prototype.tojson')
const history = createHistory()
const StoreInstance = Store({
  platforms: new Map(),
  games: []
}, history)

ReactDOM.render(
  <Provider store={StoreInstance}>
    <App history={history}/>
  </Provider>,
 document.getElementById('root')
)
