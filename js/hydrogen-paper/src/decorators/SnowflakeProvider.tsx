import * as React from 'react'
import * as PropTypes from 'prop-types'
import { connect, Dispatch } from 'react-redux'
import RootState from 'state/RootState'
import { Platform, Game } from 'snowflake-remoting'
import { Action } from 'redux'
import * as Actions from 'state/Actions'

export type SnowflakeContext = {
  Snowflake: {
    Platforms: Map<string, Platform>,
    Games: Game[],
    Dispatch: Dispatch<Action>
  }
}

function mapDispatchToProps<A extends Action> (dispatch: Dispatch<A>) {
  return {
    Dispatch: dispatch
  }
}

function mapStateToProps (state: RootState): RootState {
  return {
    Platforms: state.Platforms,
    Games: state.Games
  }
}

class SnowflakeProvider extends React.Component<RootState & { Dispatch: Dispatch<Action> }> {
  static childContextTypes = {
    Snowflake: PropTypes.object
  }

  componentDidMount () {
    this.props.Dispatch(Actions.refreshGames)
    this.props.Dispatch(Actions.refreshPlatforms)
  }

  getChildContext (): SnowflakeContext {
    return {
      Snowflake: {
        Platforms: this.props.Platforms,
        Games: this.props.Games,
        Dispatch: this.props.Dispatch
      }
    }
  }

  render (): any {
    return (
      <div>
        { this.props.children }
      </div>
    )
  }
}

export default connect(mapStateToProps, mapDispatchToProps)(SnowflakeProvider)
