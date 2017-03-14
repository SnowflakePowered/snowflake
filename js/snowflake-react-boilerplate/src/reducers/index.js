// src/reducers/index.js
import platforms from './platforms'
import games from './games'
import { combineReducers } from 'redux';
const rootReducer = combineReducers({
    platforms,
    games
});
export default rootReducer;