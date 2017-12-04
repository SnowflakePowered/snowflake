import * as React from 'react'
import * as PropTypes from 'prop-types'
import { connect, Dispatch } from 'react-redux'
import State from 'state/State'
import Snowflake, {
  Platform,
  Game,
  ConfigurationCollection
} from 'snowflake-remoting'
import { Action } from 'redux'
import * as Actions from 'state/Actions'
import * as Selectors from 'state/Selectors'
import { ConfigurationKey } from 'support/ConfigurationKey'
type SnowflakeContext = {
  Snowflake: SnowflakeData;
}

/**
 * Represents the current store of data from the running Snowflake instance
 */
export type SnowflakeData = {
  ElementLoadingStates: (elementId: string) => boolean
  /**
   * A map of Stone platforms from ID to Platform
   */
  Games: { [gameGuid: string]: Game }
  Platforms: { [platformId: string]: Platform }
  /**
   * The currently active platform
   */
  ActivePlatform: Platform,
  /**
   * The currently active Game
   */
  ActiveGame: Game,
  /**
   * The games for the currently active platform
   */
  ActivePlatformGames: Game[],
  ActiveEmulatorConfiguration: { key: ConfigurationKey, config: ConfigurationCollection }
  /**
   * The main action dispatcher
   */
  Dispatch?: Dispatch<Action>,
  /**
   * Access to the main Snowflake API
   */
  Api: Snowflake
}

const snowflake = new Snowflake()

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
    ActiveGame: Selectors.activeGameSelector(state),
    ActivePlatformGames: Selectors.activePlatformGamesSelector(state),
    ActiveEmulatorConfiguration: Selectors.activeEmulatorConfigurationSelector(state),
    ElementLoadingStates: Selectors.isElementLoadingSelector(state),
    Api: snowflake
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

  render (): JSX.Element {
    console.log(this.props)
    return (
      <div>
        { this.props.children }
      </div>
    )
  }
}

export default connect(mapStateToProps, mapDispatchToProps)(SnowflakeProvider)
