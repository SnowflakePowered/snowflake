import * as React from 'react'
import TextField from 'material-ui/TextField'

import ConfigurationWidget from 'components/ConfigurationWidget/ConfigurationWidget'
import { ConfigurationOption } from 'snowflake-remoting'
import { ConfigurationKey } from 'support/ConfigurationKey'

type StringWidgetProps = {
  stringOption: ConfigurationOption,
  configkey: ConfigurationKey,
  isLoading: boolean,
  onValueChange: (newValue: string) => void
}
// <Switch checked={stringOption.Value.Value} onChange={valueChangeHandler}/>
// todo: Refactor this out to a container.
const StringWidget: React.SFC<StringWidgetProps> = ({stringOption, isLoading, configkey, onValueChange}) => {
  const valueChangeHandler = (event: Event) => {
    onValueChange((event.target as any).value)
  }

  return (
    <ConfigurationWidget name={stringOption.Descriptor.DisplayName} description={stringOption.Descriptor.Description} isLoading={isLoading}>
     <TextField value={stringOption.Value.Value} onChange={valueChangeHandler}/>
    </ConfigurationWidget>
  )
}

export default StringWidget
