import * as React from 'react'
import * as ReactDOM from 'react-dom'
import App from 'init/App'
// import registerServiceWorker from './registerServiceWorker';
import './index.css'
import { Provider } from 'react-redux'
import Store from 'state/Store'
import { ConnectedRouter as Router } from 'react-router-redux'

import { createBrowserHistory } from 'history'
const history = createBrowserHistory()
console.log(history)
const storeInstance = Store(history)

ReactDOM.render(
  <Provider store={storeInstance}>
    <Router history={history}>
      <App />
    </Router>
  </Provider>,
  document.getElementById('root') as HTMLElement
)
// registerServiceWorker();
