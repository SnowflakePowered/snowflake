import * as React from 'react'
import * as PropTypes from 'prop-types'
import { connect, Dispatch } from 'react-redux'
import State from 'state/State'
import { Platform, Game } from 'snowflake-remoting'
import { Action } from 'redux'
import * as Actions from 'state/Actions'
import * as Selectors from 'state/Selectors'

type SnowflakeContext = {
  Snowflake: SnowflakeData
}

export type SnowflakeData = {
  Platforms: Map<string, Platform>,
  Games: Game[],
  ActivePlatform: Platform,
  ActiveGame: Game,
  Dispatch?: Dispatch<Action>
}

function mapDispatchToProps<A extends Action> (dispatch: Dispatch<A>) {
  return {
    Dispatch: dispatch
  }
}

function mapStateToProps (state: State): SnowflakeData {
  return {
    Platforms: Selectors.platformsSelector(state),
    Games: Selectors.gamesSelector(state),
    ActivePlatform: Selectors.activePlatformSelector(state)!,
    ActiveGame: Selectors.activeGameSelector(state)
  }
}

class SnowflakeProvider extends React.Component<SnowflakeData & { Dispatch: Dispatch<Action> }> {
  static childContextTypes = {
    Snowflake: PropTypes.object
  }

  componentDidMount () {
    this.props.Dispatch(Actions.refreshGames)
    this.props.Dispatch(Actions.refreshPlatforms)
  }

  getChildContext (): SnowflakeContext {
    return {
      Snowflake: this.props
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
