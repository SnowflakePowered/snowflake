// src/reducers/index.js
import platforms from './platforms'
import games from './games'
import ui from './ui'
import { combineReducers } from 'redux'
const rootReducer = combineReducers({
  platforms,
  games,
  ui
})
export default rootReducer
