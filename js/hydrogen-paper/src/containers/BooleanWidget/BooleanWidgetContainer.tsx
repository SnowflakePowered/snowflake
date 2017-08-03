import * as React from 'react'
import withSnowflake, { SnowflakeProps } from 'decorators/withSnowflake'
import { ConfigurationOption, ConfigurationValue } from 'snowflake-remoting'
import { ConfigurationKey } from 'support/ConfigurationKey'

import BooleanWidget from 'components/BooleanWidget/BooleanWidget'

type BooleanWidgetProps = {
  booleanOption: ConfigurationOption,
  configKey: ConfigurationKey,
  handleUpdate: (configKey: ConfigurationKey, newValue: ConfigurationValue, option: ConfigurationOption) => void
}
// todo: Refactor this out to a container.
const BooleanWidgetContainer: React.SFC<SnowflakeProps & BooleanWidgetProps> = ({snowflake, booleanOption, configKey, handleUpdate}) => {
  const valueChangeHandler = (value: boolean) => {
    const newValue: ConfigurationValue = { ...booleanOption.Value, Value: value}
    handleUpdate(configKey, newValue, booleanOption)
  }
  const isLoading = snowflake.ElementLoadingStates(booleanOption.Value.Guid)
  return (
    <BooleanWidget booleanOption={booleanOption}
                   configkey={configKey}
                   onValueChange={valueChangeHandler}
                   isLoading={isLoading} />
  )
}

export default withSnowflake(BooleanWidgetContainer)
