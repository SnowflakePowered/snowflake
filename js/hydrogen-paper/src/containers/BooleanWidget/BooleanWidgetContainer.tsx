import * as React from 'react'
import withSnowflake, { SnowflakeProps } from 'decorators/withSnowflake'
import { ConfigurationOption, ConfigurationValue } from 'snowflake-remoting'
import * as Actions from 'state/Actions'
import { ConfigurationKey } from 'support/ConfigurationKey'

import BooleanWidget from 'components/BooleanWidget/BooleanWidget'

type BooleanWidgetProps = {
  booleanOption: ConfigurationOption,
  configKey: ConfigurationKey
}
// todo: Refactor this out to a container.
const BooleanWidgetContainer: React.SFC<SnowflakeProps & BooleanWidgetProps> = ({snowflake, booleanOption, configKey}) => {
  const valueChangeHandler = (value: boolean) => {
    const newValue: ConfigurationValue = { ...booleanOption.Value, Value: value}
    snowflake.Dispatch!(Actions.refreshGameConfiguration.started({
      configKey: configKey,
      newValue: newValue
    }))
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
