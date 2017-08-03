import * as React from 'react'
import { DetailedHTMLProps, InputHTMLAttributes }  from 'react'
import TextField from 'material-ui/TextField'

import ConfigurationWidget from 'components/ConfigurationWidget/ConfigurationWidget'
import { ConfigurationOption } from 'snowflake-remoting'
import { ConfigurationKey } from 'support/ConfigurationKey'

type IntegerWidgetProps = {
  integerOption: ConfigurationOption,
  configkey: ConfigurationKey,
  isLoading: boolean,
  onValueChange: (newValue: number) => void
}

const NumberInputProps: (integerOption: ConfigurationOption) => DetailedHTMLProps<InputHTMLAttributes<HTMLInputElement>, HTMLInputElement> = (integerOption) => {
  const infinite = (integerOption.Descriptor.Max === 0 && integerOption.Descriptor.Min === 0)
  return {
    step: integerOption.Descriptor.Increment,
    max: infinite ? Number.MAX_SAFE_INTEGER : integerOption.Descriptor.Max,
    min: infinite ? Number.MIN_SAFE_INTEGER: integerOption.Descriptor.Min
  } 
}

// <Switch checked={stringOption.Value.Value} onChange={valueChangeHandler}/>
// todo: Refactor this out to a container.
const IntegerWidget: React.SFC<IntegerWidgetProps> = ({integerOption, isLoading, configkey, onValueChange}) => {
  const valueChangeHandler = (event: Event) => {
    onValueChange((event.target as any).value)
  }
  const valueFixHandler = (event: Event) => {
    const value: number = (event.target as any).value
    const roundedValue = Math.round(value) || 0
    onValueChange(roundedValue || 0)
  }
  const inputProps = NumberInputProps(integerOption)
  return (
    <ConfigurationWidget name={integerOption.Descriptor.DisplayName} description={integerOption.Descriptor.Description} isLoading={isLoading}>
     <TextField value={integerOption.Value.Value} onBlur={valueFixHandler} onChange={valueChangeHandler} type='number' inputProps={inputProps}/>
    </ConfigurationWidget>
  )
}

export default IntegerWidget
