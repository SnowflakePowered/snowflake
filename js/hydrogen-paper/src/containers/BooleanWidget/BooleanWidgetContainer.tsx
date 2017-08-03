import * as React from 'react'
import withSnowflake, { SnowflakeProps } from 'decorators/withSnowflake'
import { ConfigurationOption, ConfigurationValue } from 'snowflake-remoting'
import { ConfigurationKey } from 'support/ConfigurationKey'

import BooleanWidget from 'components/BooleanWidget/BooleanWidget'

type BooleanWidgetProps = {
  option: ConfigurationOption,
  configKey: ConfigurationKey,
  handleUpdate: (configKey: ConfigurationKey, newValue: ConfigurationValue, option: ConfigurationOption) => void
}
// todo: Refactor this out to a container.
const BooleanWidgetContainer: React.SFC<SnowflakeProps & BooleanWidgetProps> = ({snowflake, option, configKey, handleUpdate}) => {
  const valueChangeHandler = (value: boolean) => {
    const newValue: ConfigurationValue = { ...option.Value, Value: value}
    handleUpdate(configKey, newValue, option)
  }
  const isLoading = snowflake.ElementLoadingStates(option.Value.Guid)
  return (
    <BooleanWidget booleanOption={option}
                   configkey={configKey}
                   onValueChange={valueChangeHandler}
                   isLoading={isLoading} />
  )
}

export default withSnowflake(BooleanWidgetContainer)
