import { createStore, applyMiddleware } from 'redux'
import createSagaMiddleware from 'redux-saga'
import rootSaga from 'state/Sagas'
import reducer from 'state/Reducer'
import Snowflake from 'snowflake-remoting'
import State from 'state/State'
import { composeWithDevTools } from 'redux-devtools-extension'
require('map.prototype.tojson')

const snowflake = new Snowflake()

const sagaMiddleware = createSagaMiddleware()
// mount it on the Store
const store = createStore<State>(
  reducer,
  composeWithDevTools(applyMiddleware(sagaMiddleware))
)

sagaMiddleware.run(rootSaga, snowflake)

export default store
