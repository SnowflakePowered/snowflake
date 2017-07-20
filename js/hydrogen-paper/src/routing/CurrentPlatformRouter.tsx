import * as React from 'react'
import { withRouter, RouteComponentProps } from 'react-router'
import * as queryString from 'query-string'
import withSnowflake, { SnowflakeProps } from 'decorators/withSnowflake'
import { setCurrentPlatform } from 'state/Actions'

class CurrentPlatformRouter extends React.Component<SnowflakeProps & RouteComponentProps<any>> {
  componentDidMount () {
    console.log()
    console.log(this.props.snowflake)
  }

  render () {
    let { platform }: { platform: string} = queryString.parse(this.props.location.search)
    let platformObj = this.props.snowflake.Snowflake.Platforms.get(platform)!
    this.props.snowflake.Snowflake.Dispatch(setCurrentPlatform(platformObj))
    return null
  }
}

export default withSnowflake(withRouter(CurrentPlatformRouter))
