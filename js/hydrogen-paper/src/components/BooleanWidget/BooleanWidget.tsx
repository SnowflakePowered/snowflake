import * as React from 'react'
import Switch from 'material-ui/Switch'

import ConfigurationWidget from 'components/ConfigurationWidget/ConfigurationWidget'
import withSnowflake, { SnowflakeProps } from 'decorators/withSnowflake'
import { ConfigurationOption, ConfigurationValue } from "snowflake-remoting";
import * as Actions from 'state/Actions'
import { ConfigurationKey } from 'support/ConfigurationKey'

export type BooleanWidgetProps = {
  option: ConfigurationOption,
  configkey: ConfigurationKey,
  onValueChange?: (event: Event) => void
}
// todo: Refactor this out to a container.
const BooleanWidget: React.SFC<SnowflakeProps & BooleanWidgetProps> = ({snowflake, option, configkey, onValueChange}) => {
  const valueChangeHandler = (event: Event, value: boolean) => {
    const newValue: ConfigurationValue = { ...option.Value, Value: value}
    snowflake.Dispatch!(Actions.refreshGameConfiguration.started({
      configKey: configkey,
      newValue: newValue
    }))
  }

  return (
    <ConfigurationWidget name={option.Descriptor.DisplayName} description={option.Descriptor.Description}>
      <Switch checked={option.Value.Value} onChange={valueChangeHandler}/>
    </ConfigurationWidget>
  )
}

export default withSnowflake(BooleanWidget)
