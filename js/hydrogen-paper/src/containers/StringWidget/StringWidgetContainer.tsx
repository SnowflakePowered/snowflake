import * as React from 'react'
import withSnowflake, { SnowflakeProps } from 'decorators/withSnowflake'
import { ConfigurationOption, ConfigurationValue } from 'snowflake-remoting'
import { ConfigurationKey } from 'support/ConfigurationKey'

import StringWidget from 'components/StringWidget/StringWidget'

type StringWidgetProps = {
  stringOption: ConfigurationOption,
  configKey: ConfigurationKey,
  handleUpdate: (configKey: ConfigurationKey, newValue: ConfigurationValue) => void
}
// todo: Refactor this out to a container.
const StringWidgetContainer: React.SFC<SnowflakeProps & StringWidgetProps> = ({snowflake, stringOption, configKey, handleUpdate}) => {
  const valueChangeHandler = (value: string) => {
    const newValue: ConfigurationValue = { ...stringOption.Value, Value: value}
    handleUpdate(configKey, newValue)
  }
  const isLoading = snowflake.ElementLoadingStates(stringOption.Value.Guid)
  return (
    <StringWidget stringOption={stringOption}
                  configkey={configKey}
                  onValueChange={valueChangeHandler}
                  isLoading={isLoading} />
  )
}

export default withSnowflake(StringWidgetContainer)
