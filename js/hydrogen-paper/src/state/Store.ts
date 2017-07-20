import { createStore, applyMiddleware } from 'redux'
import createSagaMiddleware from 'redux-saga'
import rootSaga from './Sagas'
import reducer from './Reducer'
import Snowflake from 'snowflake-remoting'
import RootState from './RootState'
import { composeWithDevTools } from 'redux-devtools-extension'
require('map.prototype.tojson')

const snowflake = new Snowflake()

const sagaMiddleware = createSagaMiddleware()
// mount it on the Store
const store = createStore<RootState>(
  reducer,
  composeWithDevTools(applyMiddleware(sagaMiddleware))
)

sagaMiddleware.run(rootSaga, snowflake)

export default store
