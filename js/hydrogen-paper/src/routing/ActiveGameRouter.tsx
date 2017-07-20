import * as React from 'react'
import { withRouter, RouteComponentProps } from 'react-router'
import * as queryString from 'query-string'
import withSnowflake, { SnowflakeProps } from 'decorators/withSnowflake'
import { setActiveGame } from 'state/Actions'

class CurrentGameRouter extends React.Component<SnowflakeProps & RouteComponentProps<{}>> {
  render () {
    let { game }: { game: string} = queryString.parse(this.props.location.search)
    this.props.snowflake.Dispatch!(setActiveGame(game))
    console.log(this.props.snowflake)
    return null
  }
}

export default withSnowflake(withRouter(CurrentGameRouter))
