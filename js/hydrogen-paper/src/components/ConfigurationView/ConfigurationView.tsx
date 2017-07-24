import * as React from 'react'
import withSnowflake, { SnowflakeProps } from 'decorators/withSnowflake'
import { NoProps } from 'support/NoProps'
import { ConfigurationSection } from 'snowflake-remoting/dist/snowflake'
// import BooleanWidget from 'components/BooleanWidget/BooleanWidget'

const ConfigurationSection: React.SFC<{config: ConfigurationSection}> = ({config}) => {
  return (
    <div>
    <div>{config ? config.Descriptor.DisplayName : ''}</div>
    {Object.values(config.Configuration).map(c => <div>{c.Descriptor.DisplayName}</div>)}
    </div>
  )
}

class ConfigurationView extends React.Component<NoProps & SnowflakeProps> {
  render () {
    // todo: use get.
    if (!this.props.snowflake.ActiveEmulatorConfiguration) return <div/>
    return (
      <div>
        {Object.values(this.props.snowflake.ActiveEmulatorConfiguration)
            .map(c => <ConfigurationSection config={c}/>)}
      </div>
    )
  }
}

export default withSnowflake(ConfigurationView)
