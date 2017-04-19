import React from 'react'
import wrapDisplayName from 'recompose/wrapDisplayName'
import _ from 'lodash'

const styleable = (WrappedComponent) => {
  return class extends React.Component {
    static displayName = wrapDisplayName(WrappedComponent, 'Styleable')
    render () {
      return (
        <div className={this.props.className}>
          <WrappedComponent {..._.omit(this.props, 'className')}/>
        </div>
      )
    }
  }
}

export default styleable
