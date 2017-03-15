import React, { Component } from 'react'
import UIFrame from "./components/uiframe"
import './App.css'
import Test from './components/test'
import {
  BrowserRouter as Router,
  Route
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
    const platforms = () => <Test platforms={this.props.platforms} games={this.props.games}/>
    return (
      <Router >
        <UIFrame>
          <Route path="/platforms" component={platforms}/>
        </UIFrame>
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
