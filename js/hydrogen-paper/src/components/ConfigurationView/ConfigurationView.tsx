import * as React from 'react'
import { ConfigurationSection, ConfigurationOption, ConfigurationValue, ConfigurationCollection } from 'snowflake-remoting'
import BooleanWidget from 'containers/BooleanWidget/BooleanWidgetContainer'
import StringWidget from 'containers/StringWidget/StringWidgetContainer'
import IntegerWidget from 'containers/IntegerWidget/IntegerWidgetContainer'
import DecimalWidget from 'containers/DecimalWidget/DecimalWidgetContainer'
import { ConfigurationKey } from 'support/ConfigurationKey'
import Button from 'material-ui/Button'

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

type ConfigurationViewProps = {
  handleSave: () => void,
  emulatorConfig: { key?: ConfigurationKey, config?: ConfigurationCollection },
  handleUpdate: (sectionName: string) => (optionName: string) => (configKey: ConfigurationKey, newValue: ConfigurationValue) => void
}

const ConfigurationView: React.SFC<ConfigurationViewProps> = ({handleSave, handleUpdate, emulatorConfig}) =>  {
    // todo: use get.
    if (!emulatorConfig.config) return <div/>
    const key = emulatorConfig.key!
    return (
      <div>
        <Button onClick={handleSave}>Save Changes</Button>
        {Object.entries(emulatorConfig.config)
            .map(([section, c]) => <ConfigurationSectionView configKey={key} config={c} handleUpdate={handleUpdate(section)}/>)}
      </div>
    )
  
}

export default ConfigurationView
