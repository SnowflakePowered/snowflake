// src/reducers/index.js
import platforms from './platforms'
import games from './games'
import state from './state'

import { routerReducer } from 'react-router-redux'
import { combineReducers } from 'redux'

const rootReducer = combineReducers({
  platforms,
  games,
  state,
  router: routerReducer
})
export default rootReducer
