import * as React from 'react'
import withSnowflake, { SnowflakeProps } from 'decorators/withSnowflake'
import { NoProps } from 'support/NoProps'
import { ConfigurationSection, ConfigurationOption } from 'snowflake-remoting'
import BooleanWidget from 'containers/BooleanWidget/BooleanWidgetContainer'
import StringWidget from 'containers/StringWidget/StringWidgetContainer'

import { ConfigurationKey } from 'support/ConfigurationKey'

const ConfigurationOptionView: React.SFC<{config: ConfigurationOption, configkey: ConfigurationKey}> = ({config, configkey}) => {
  if (config.Descriptor.Type === 'boolean') {
    return <BooleanWidget booleanOption={config} configKey={configkey} />
  }
  if (config.Descriptor.Type === 'string') {
    return <StringWidget stringOption={config} configKey={configkey} />
  }
  return (<div/>)
}

const ConfigurationSectionView: React.SFC<{config: ConfigurationSection, configkey: ConfigurationKey}> = ({config, configkey}) => {
  return (
    <div>
    <div>{config ? config.Descriptor.DisplayName : ''}</div>
    {Object.values(config.Configuration).map(c => <ConfigurationOptionView config={c} configkey={configkey}/>)}
    </div>
  )
}

class ConfigurationView extends React.Component<NoProps & SnowflakeProps> {
  render () {
    // todo: use get.
    if (!this.props.snowflake.ActiveEmulatorConfiguration.config) return <div/>
    const key = this.props.snowflake.ActiveEmulatorConfiguration.key
    return (
      <div>
        {Object.values(this.props.snowflake.ActiveEmulatorConfiguration.config)
            .map(c => <ConfigurationSectionView configkey={key} config={c}/>)}
      </div>
    )
  }
}

export default withSnowflake(ConfigurationView)
