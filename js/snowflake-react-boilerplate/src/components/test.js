import React, { Component } from 'react'
import { bindActionCreators } from 'redux'
import { connect } from 'react-redux'
import * as platformActions from '../actions/platforms'
import * as gameActions from '../actions/games'

class Test extends Component {
  constructor (props) {
    super(props)
    this.state = {

    }
  }

  platformList () {
    if (this.props.platforms.entries !== undefined) {
      return ([...this.props.platforms.values()].map(p => <li key={p.PlatformID}>{ p.FriendlyName }</li>))
    }
  }

  render () {
    console.log(this.props.platforms)
    return (
      <div className="Tester">
       <button onClick={() => {
         this.props.dispatch(this.props.platformActions.beginUpdatePlatforms)
         this.props.dispatch(this.props.gameActions.beginUpdateGames)
       }
        }>Init</button>

        <div className="Platforms">
            <ul>
              { this.platformList() }
            </ul>
        </div>
      </div>
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
    platformActions: bindActionCreators(platformActions, dispatch),
    gameActions: bindActionCreators(gameActions, dispatch),
    dispatch
  }
}
export default connect(mapStateToProps, mapDispatchToProps)(Test)
