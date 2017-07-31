import * as React from 'react'
import withSnowflake, { SnowflakeProps } from 'decorators/withSnowflake'
import { ConfigurationOption, ConfigurationValue } from 'snowflake-remoting'
import * as Actions from 'state/Actions'
import { ConfigurationKey } from 'support/ConfigurationKey'

import StringWidget from 'components/StringWidget/StringWidget'

type StringWidgetProps = {
  stringOption: ConfigurationOption,
  configKey: ConfigurationKey
}
// todo: Refactor this out to a container.
const StringWidgetContainer: React.SFC<SnowflakeProps & StringWidgetProps> = ({snowflake, stringOption, configKey}) => {
  const valueChangeHandler = (value: string) => {
    const newValue: ConfigurationValue = { ...stringOption.Value, Value: value}
    snowflake.Dispatch!(Actions.refreshGameConfiguration.started({
      configKey: configKey,
      newValue: newValue
    }))
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
