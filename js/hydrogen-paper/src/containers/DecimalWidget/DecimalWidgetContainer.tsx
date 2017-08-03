import * as React from 'react'
import withSnowflake, { SnowflakeProps } from 'decorators/withSnowflake'
import { ConfigurationOption, ConfigurationValue } from 'snowflake-remoting'
import { ConfigurationKey } from 'support/ConfigurationKey'

import IntegerWidget from 'components/IntegerWidget/IntegerWidget'

type DecimalWidgetProps = {
  option: ConfigurationOption,
  configKey: ConfigurationKey,
  handleUpdate: (configKey: ConfigurationKey, newValue: ConfigurationValue) => void
}
// todo: Refactor this out to a container.
const DecimalWidgetContainer: React.SFC<SnowflakeProps & DecimalWidgetProps> = ({snowflake, option, configKey, handleUpdate}) => {
  const valueChangeHandler = (value: number) => {
    const newValue: ConfigurationValue = { ...option.Value, Value: value}
    handleUpdate(configKey, newValue)
  }
  const isLoading = snowflake.ElementLoadingStates(option.Value.Guid)
  return (
    <IntegerWidget integerOption={option}
                  configkey={configKey}
                  onValueChange={valueChangeHandler}
                  isLoading={isLoading} />
  )
}

export default withSnowflake(DecimalWidgetContainer)
