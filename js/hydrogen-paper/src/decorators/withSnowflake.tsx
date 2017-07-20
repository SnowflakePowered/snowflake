import * as React from 'react'
import * as PropTypes from 'prop-types'
import { wrapDisplayName } from 'recompose'
import { SnowflakeData } from 'state/SnowflakeProvider'

export type SnowflakeProps = {
  snowflake: SnowflakeData;
}

type SnowflakeContext = {
  Snowflake: SnowflakeData
}

function objectWithoutKey<T extends object> (object: T, key: string): T {
  const {[key]: deletedKey, ...otherKeys}: object = object
  return otherKeys as T
}

const withSnowflake = <TOriginalProps extends {}>(
  WrappedComponent:
    | React.ComponentClass<TOriginalProps & SnowflakeProps>
    | React.StatelessComponent<TOriginalProps & SnowflakeProps>
): React.ComponentClass<any & SnowflakeProps> => {
  return class extends React.Component<any & SnowflakeProps, {}> {
    static contextTypes = {
      Snowflake: PropTypes.object
    }
    static displayName = wrapDisplayName(WrappedComponent, 'Snowflake')
    context: SnowflakeContext
    render (): JSX.Element {
      return (
        <WrappedComponent
          {...this.props}
          snowflake={ objectWithoutKey(this.context.Snowflake, 'children') }
        />
      )
    }
  }
}
export default withSnowflake
