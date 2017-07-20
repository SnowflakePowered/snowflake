import * as React from 'react'
import * as ReactDOM from 'react-dom'
import App from 'init/App'
// import registerServiceWorker from './registerServiceWorker';
import './index.css'
import { Provider } from 'react-redux'
import Store from 'state/Store'

ReactDOM.render(
  <Provider store={Store}>
    <App />
  </Provider>,
  document.getElementById('root') as HTMLElement
)
// registerServiceWorker();
