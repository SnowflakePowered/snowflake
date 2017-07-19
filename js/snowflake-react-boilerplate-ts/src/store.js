import { createStore, applyMiddleware } from 'redux'
import { composeWithDevTools } from 'redux-devtools-extension'
import thunk from 'redux-thunk'
import Snowflake from 'snowflake-remoting'
import rootReducer from './reducers'
import { routerMiddleware } from 'react-router-redux'

const snowflake = new Snowflake()

export default (initialState, history) => {
  return createStore(rootReducer, initialState, composeWithDevTools(
        applyMiddleware(thunk.withExtraArgument(snowflake), routerMiddleware(history))
    ))
}
