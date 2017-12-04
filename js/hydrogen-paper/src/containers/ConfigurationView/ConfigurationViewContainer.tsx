import * as React from 'react'

import withSnowflake, { SnowflakeProps } from 'decorators/withSnowflake'
import { ConfigurationCollection, ConfigurationValue } from 'snowflake-remoting'
import { ConfigurationKey } from 'support/ConfigurationKey'
import { OrderedMap } from 'immutable'
import { NoProps } from 'support/NoProps'
import * as Actions from 'state/Actions'

import ConfigurationView from 'components/ConfigurationView/ConfigurationView'

type ConfigurationUpdate = {
  configKey: ConfigurationKey,
  newValue: ConfigurationValue
}

type ConfigurationViewState = {
  emulatorConfig: { key?: ConfigurationKey, config?: ConfigurationCollection },
  delta: OrderedMap<string, ConfigurationUpdate>
}

class ConfigurationViewContainer extends React.Component<NoProps & SnowflakeProps, ConfigurationViewState> {
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
    return (<ConfigurationView handleSave={this.handleSave} emulatorConfig={this.state.emulatorConfig} handleUpdate={this.handleUpdate} />)
  }
}

export default withSnowflake(ConfigurationViewContainer)