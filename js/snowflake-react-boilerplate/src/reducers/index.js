// src/reducers/index.js
import platforms from './platforms/'
import games from './games/'
import { routerReducer } from 'react-router-redux'
import { combineReducers } from 'redux'
const rootReducer = combineReducers({
  platforms,
  games,
  router: routerReducer
})
export default rootReducer
