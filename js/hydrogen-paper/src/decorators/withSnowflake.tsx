import * as React from 'react'
import * as PropTypes from 'prop-types'
import { wrapDisplayName } from 'recompose'
import { SnowflakeData } from 'state/SnowflakeProvider'

export type SnowflakeProps = {
  snowflake: SnowflakeData
}

type SnowflakeContext = {
  Snowflake: SnowflakeData
}

function objectWithoutKey<T extends object> (object: T, key: string): T {
  const {[key]: deletedKey, ...otherKeys}: object = object
  return otherKeys as T
}

/**
 * Injects current Snowflake data into the specified React Component.
 * This HOC always returns a ComponentClass.
 * @param WrappedComponent The component to inject
 */
const withSnowflake = <T extends {}>(
  WrappedComponent:
    | React.ComponentClass<T & SnowflakeProps>
    | React.StatelessComponent<T & SnowflakeProps>
): React.ComponentClass<T> => {
  return class extends React.Component<T & SnowflakeProps, {}> {
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
