import * as React from 'react'
import { wrapDisplayName } from 'recompose'
import { queryParamsSelector } from 'state/Selectors'
import { RouterState } from 'react-router-redux'
import { connect } from 'react-redux'

export type QueryProps = {
  query: { [key: string]: string | string[] }
}

function mapStateToProps (state: RouterState): QueryProps {
  return {
    query: queryParamsSelector(state)
  }
}

const withQuery = <TOriginalProps extends {}>(
  WrappedComponent:
    | React.ComponentClass<TOriginalProps & QueryProps>
    | React.StatelessComponent<TOriginalProps & QueryProps>
): React.ComponentClass<any & QueryProps> => {
  return connect(mapStateToProps)(class extends React.Component<any & QueryProps, {}> {
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
