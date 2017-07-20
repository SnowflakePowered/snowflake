import * as React from 'react'
import * as PropTypes from 'prop-types'
import { wrapDisplayName } from 'recompose'
import { SnowflakeContext } from 'decorators/SnowflakeProvider'

export type SnowflakeProps = {
  snowflake: SnowflakeContext;
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
          snowflake={{
            Snowflake: this.context.Snowflake
          }}
        />
      )
    }
  }
}
export default withSnowflake
