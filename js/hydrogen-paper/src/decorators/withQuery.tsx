import * as React from 'react'
import { wrapDisplayName } from 'recompose'
import { queryParamsSelector } from 'state/Selectors'
import { RouterState } from 'react-router-redux'
import { connect } from 'react-redux'

export type QueryProps = {
  query?: { [key: string]: string | string[] }
}

function mapStateToProps <T> (state: RouterState, ownProps: T): QueryProps {
  return {
    query: queryParamsSelector(state)
  }
}

const withQuery = <T extends {}>(
  WrappedComponent:
    | React.ComponentClass<T & QueryProps>
    | React.StatelessComponent<T & QueryProps>
): React.ComponentClass<T> => {
  return connect(mapStateToProps)(class extends React.Component<T & QueryProps, {}> {
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
