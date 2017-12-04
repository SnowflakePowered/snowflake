import * as React from 'react'
import Switch from 'material-ui/Switch'

import ConfigurationWidget from 'components/ConfigurationWidget/ConfigurationWidget'
import { ConfigurationOption } from 'snowflake-remoting'
import { ConfigurationKey } from 'support/ConfigurationKey'

type BooleanWidgetProps = {
  booleanOption: ConfigurationOption,
  configkey: ConfigurationKey,
  isLoading: boolean,
  onValueChange: (newValue: boolean) => void
}
// todo: Refactor this out to a container.
const BooleanWidget: React.SFC<BooleanWidgetProps> = ({booleanOption, isLoading, configkey, onValueChange}) => {
  const valueChangeHandler = (event: Event, value: boolean) => {
    onValueChange(value)
  }

  return (
    <ConfigurationWidget name={booleanOption.Descriptor.DisplayName} description={booleanOption.Descriptor.Description} isLoading={isLoading}>
      <Switch checked={booleanOption.Value.Value} onChange={valueChangeHandler}/>
    </ConfigurationWidget>
  )
}

export default BooleanWidget
