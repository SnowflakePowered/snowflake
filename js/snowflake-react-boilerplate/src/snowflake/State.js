import React, { Component } from 'react'
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'
import * as platformActions from '../actions/platforms'
import * as gameActions from '../actions/games'
import getDisplayName from 'recompose/getDisplayName'

const State = (AppComponent) => {
  function mapStateToProps (state, props) {
    return {
      stone: {
        platforms: state.platforms
      },
      games: state.games,
      ui: state.ui
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

  return connect(mapStateToProps, mapDispatchToProps)(class extends Component {
    static displayName = `State(${getDisplayName(AppComponent)})`
    componentWillMount () {
      this.props.actions.platforms.beginRefreshPlatforms()
      this.props.actions.games.beginRefreshGames()
    }

    render () {
      return (
        <AppComponent {...this.props} />
      )
    }
  })
}

export default State
