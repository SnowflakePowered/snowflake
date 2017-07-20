import * as React from 'react'
import { withRouter, RouteComponentProps } from 'react-router'
import * as queryString from 'query-string'
import withSnowflake, { SnowflakeProps } from 'decorators/withSnowflake'
import { setActivePlatform } from 'state/Actions'

class CurrentPlatformRouter extends React.Component<SnowflakeProps & RouteComponentProps<any>> {
  render () {
    let { platform }: { platform: string} = queryString.parse(this.props.location.search)
    this.props.snowflake.Dispatch!(setActivePlatform(platform))
    console.log(this.props.snowflake)
    return null
  }
}

export default withSnowflake(withRouter(CurrentPlatformRouter))
