import * as React from 'react'
import { wrapDisplayName } from 'recompose'
import { queryParamsSelector } from 'state/Selectors'
import { RouterState } from 'react-router-redux'
import { connect } from 'react-redux'

export type QueryProps = {
  query?: { [key: string]: string | string[] }
}

function mapStateToProps <OwnProps> (state: RouterState, ownProps: OwnProps): QueryProps {
  return {
    query: queryParamsSelector(state)
  }
}

const withQuery = <OwnProps extends {}>(
  WrappedComponent:
    | React.ComponentClass<OwnProps & QueryProps>
    | React.StatelessComponent<OwnProps & QueryProps>
): React.ComponentClass<OwnProps & QueryProps> => {
  return connect(mapStateToProps)(class extends React.Component<OwnProps & QueryProps, {}> {
    static displayName = wrapDisplayName(WrappedComponent, 'WithQuery')
    render (): JSX.Element {
      return (
        <WrappedComponent
          {...this.props}
        />
      )
    }
  })
}
export default withQuery
