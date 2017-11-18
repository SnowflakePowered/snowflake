import * as React from 'react'
import { DetailedHTMLProps, InputHTMLAttributes }  from 'react'
import TextField from 'material-ui/TextField'

import ConfigurationWidget from 'components/ConfigurationWidget/ConfigurationWidget'
import { ConfigurationOption } from 'snowflake-remoting'
import { ConfigurationKey } from 'support/ConfigurationKey'

type DecimalWidgetProps = {
  decimalOption: ConfigurationOption,
  configkey: ConfigurationKey,
  isLoading: boolean,
  onValueChange: (newValue: number) => void
}

const NumberInputProps: (decimalOption: ConfigurationOption) => DetailedHTMLProps<InputHTMLAttributes<HTMLInputElement>, HTMLInputElement> = (integerOption) => {
  const infinite = (integerOption.Descriptor.Max === 0 && integerOption.Descriptor.Min === 0)
  return {
    step: integerOption.Descriptor.Increment,
    max: infinite ? Number.MAX_SAFE_INTEGER : integerOption.Descriptor.Max,
    min: infinite ? Number.MIN_SAFE_INTEGER: integerOption.Descriptor.Min
  }
}

// <Switch checked={stringOption.Value.Value} onChange={valueChangeHandler}/>
// todo: Refactor this out to a container.
const DecimalWidget: React.SFC<DecimalWidgetProps> = ({decimalOption, isLoading, configkey, onValueChange}) => {
  const valueChangeHandler = (event: Event) => {
    onValueChange((event.target as any).value)
  }
  const inputProps = NumberInputProps(decimalOption)
  return (
    <ConfigurationWidget name={decimalOption.Descriptor.DisplayName} description={decimalOption.Descriptor.Description} isLoading={isLoading}>
     <TextField value={decimalOption.Value.Value as number} onChange={valueChangeHandler} type='number' inputProps={inputProps}/>
    </ConfigurationWidget>
  )
}

export default DecimalWidget
