import { createStore, applyMiddleware } from 'redux'
import createSagaMiddleware from 'redux-saga'
import rootSaga from 'state/Sagas'
import reducer from 'state/Reducer'
import Snowflake from 'snowflake-remoting'
import State from 'state/State'
import { composeWithDevTools } from 'redux-devtools-extension'
import { History } from 'history'
import { routerMiddleware as createRouterMiddleware } from 'react-router-redux'
require('map.prototype.tojson')

// mount it on the Store
const store = (history: History) => {
  const snowflake = new Snowflake()
  const sagaMiddleware = createSagaMiddleware()
  const routerMiddleware = createRouterMiddleware(history)
  const store = createStore<State>(
      reducer,
      composeWithDevTools(applyMiddleware(sagaMiddleware, routerMiddleware))
    )
  sagaMiddleware.run(rootSaga, snowflake)
  return store
}

export default store
