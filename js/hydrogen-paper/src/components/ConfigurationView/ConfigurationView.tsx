import * as React from 'react'
import withSnowflake, { SnowflakeProps } from 'decorators/withSnowflake'
import { NoProps } from 'support/NoProps'
// import BooleanWidget from 'components/BooleanWidget/BooleanWidget'

class ConfigurationView extends React.Component<NoProps & SnowflakeProps> {
  render () {
    return (
      <div>
        Coming Soon!
      </div>
    )
  }
}

export default withSnowflake(ConfigurationView)
