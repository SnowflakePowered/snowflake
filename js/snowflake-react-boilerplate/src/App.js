import React, { Component } from 'react'
import logo from './logo.svg'
import './App.css'
import Test from './components/test'
import {
  BrowserRouter as Router
} from 'react-router-dom'
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'
import * as platformActions from './actions/platforms'
import * as gameActions from './actions/games'

class App extends Component {
  componentDidMount () {
    this.props.dispatch(this.props.actions.platforms.beginUpdatePlatforms)
    this.props.dispatch(this.props.actions.games.beginUpdateGames)
  }

  render () {
    return (
      <Router >
        <div className="App">
          <div className="App-header">
            <img src={logo} className="App-logo" alt="logo" />
            <h2>Welcome to React</h2>
          </div>
          <div className="App-intro">
            To get started, edit <code>src/App.js</code> and save to reload.
            <Test platforms={this.props.platforms} games={this.props.games}/>
          </div>
        </div>
      </Router>
    )
  }
}

function mapStateToProps (state, props) {
  return {
    platforms: state.platforms,
    games: state.games
  }
}

function mapDispatchToProps (dispatch) {
  return {
    actions: {
      platforms: bindActionCreators(platformActions, dispatch),
      games: bindActionCreators(gameActions, dispatch)
    },
    dispatch
  }
}
export default connect(mapStateToProps, mapDispatchToProps)(App)
