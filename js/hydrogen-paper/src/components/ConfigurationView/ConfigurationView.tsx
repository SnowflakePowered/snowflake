import * as React from 'react'
import withSnowflake, { SnowflakeProps } from 'decorators/withSnowflake'
import { NoProps } from 'support/NoProps'
import { ConfigurationSection, ConfigurationOption, ConfigurationValue, ConfigurationCollection } from 'snowflake-remoting'
import BooleanWidget from 'containers/BooleanWidget/BooleanWidgetContainer'
import StringWidget from 'containers/StringWidget/StringWidgetContainer'
import IntegerWidget from 'containers/IntegerWidget/IntegerWidgetContainer'
import DecimalWidget from 'containers/DecimalWidget/DecimalWidgetContainer'

import * as Actions from 'state/Actions'
import { ConfigurationKey } from 'support/ConfigurationKey'
import Button from 'material-ui/Button'
import { OrderedMap } from 'immutable'

type ConfigurationProps = {
  configKey: ConfigurationKey,
  handleUpdate: (configKey: ConfigurationKey, newValue: ConfigurationValue) => void
}

const ConfigurationOptionView: React.SFC<{config: ConfigurationOption} & ConfigurationProps> = ({config, configKey, handleUpdate}) => {
  if (config.Descriptor.Type === 'boolean') {
    return <BooleanWidget option={config} configKey={configKey} handleUpdate={handleUpdate}/>
  }
  if (config.Descriptor.Type === 'string') {
    return <StringWidget option={config} configKey={configKey} handleUpdate={handleUpdate}/>
  }
  if (config.Descriptor.Type === 'integer') {
    return <IntegerWidget option={config} configKey={configKey} handleUpdate={handleUpdate}/>
  }
  if (config.Descriptor.Type === 'decimal') {
    return <DecimalWidget option={config} configKey={configKey} handleUpdate={handleUpdate}/>
  }
  return (<div/>)
}

type ConfigurationSectionProps = {
  configKey: ConfigurationKey,
  handleUpdate: (sectionName: string) => (configKey: ConfigurationKey, newValue: ConfigurationValue) => void
}

const ConfigurationSectionView: React.SFC<{config: ConfigurationSection} & ConfigurationSectionProps> = ({config, configKey, handleUpdate}) => {
  return (
    <div>
    <div>{config ? config.Descriptor.DisplayName : ''}</div>
    {Object.entries(config.Configuration).map(([key, c]) => <ConfigurationOptionView config={c} configKey={configKey} handleUpdate={handleUpdate(key)}/>)}
    </div>
  )
}

type ConfigurationUpdate = {
  configKey: ConfigurationKey,
  newValue: ConfigurationValue
}

type ConfigurationViewState = {
  emulatorConfig: { key?: ConfigurationKey, config?: ConfigurationCollection },
  delta: OrderedMap<string, ConfigurationUpdate>
}

class ConfigurationView extends React.Component<NoProps & SnowflakeProps, ConfigurationViewState> {

  constructor (props) {
    super(props)
    this.state = {
      emulatorConfig: {},
      delta: OrderedMap()
    }
  }

  handleUpdate = (sectionName: string) => (optionName: string) => (configKey: ConfigurationKey, newValue: ConfigurationValue) => {
    // nested spread.
    this.setState({
      delta: this.state.delta.set(newValue.Guid, {configKey: configKey, newValue: newValue}),
      emulatorConfig: {
        ...this.state.emulatorConfig,
       config: {
         ...this.state.emulatorConfig.config as any,
         [sectionName]: {
           ...this.state.emulatorConfig.config![sectionName],
           Configuration: {
             ...this.state.emulatorConfig.config![sectionName].Configuration,
             [optionName]: {
               ...this.state.emulatorConfig.config![sectionName].Configuration[optionName],
               Value: newValue
             }
           }
         }
       }
      }
    })
  }

  componentDidMount () {
    this.setState({emulatorConfig: this.props.snowflake.ActiveEmulatorConfiguration})
  }

  componentWillReceiveProps (nextProps: SnowflakeProps) {
    this.setState({emulatorConfig: nextProps.snowflake.ActiveEmulatorConfiguration})
  }

  // We can diff the delta with the stored state for an optimization later on.
  handleSave = () => {
    if (this.state.delta.count() === 0) return
    const configKey = this.state.delta.first().configKey
    const values = this.state.delta.toArray().map(v => v.newValue)
    this.props.snowflake.Dispatch!(Actions.refreshGameConfigurations.started({
      configKey: configKey,
      newValues: values
    }))
    this.setState({
      ...this.state,
      delta: this.state.delta.clear()
    })
  }

  render () {
    // todo: use get.
    if (!this.state.emulatorConfig.config) return <div/>
    const key = this.state.emulatorConfig.key!
    return (
      <div>
        <Button onClick={this.handleSave}>Save Changes</Button>
        {Object.entries(this.state.emulatorConfig.config)
            .map(([section, c]) => <ConfigurationSectionView configKey={key} config={c} handleUpdate={this.handleUpdate(section)}/>)}
      </div>
    )
  }
}

export default withSnowflake(ConfigurationView)
