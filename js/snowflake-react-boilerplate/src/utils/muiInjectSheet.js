import React from 'react'
import wrapDisplayName from 'recompose/wrapDisplayName'
import getDisplayName from 'recompose/getDisplayName'
import customPropTypes from 'material-ui/utils/customPropTypes'
import { createStyleSheet } from 'jss-theme-reactor'

import _ from 'lodash'

const sanitize = (displayName) => (displayName.replace(/(\(|\[)/g, "-").replace(/(\)|\])/g, "")) //todo make this better

const muiInjectSheet = (styles) => (WrappedComponent) => {
  return class extends React.Component {
    constructor (props) {
      super(props)
      const name = sanitize(getDisplayName(WrappedComponent))
      console.log(name)
      this.styleSheet = createStyleSheet(name, (theme) => (styles))
    }

    static displayName = wrapDisplayName(WrappedComponent, 'MuiJss')

    static contextTypes = {
        styleManager: customPropTypes.muiRequired
    }

    render () {
      const classes = this.context.styleManager.render(this.styleSheet)
      console.log(this.context.styleManager)
      return (
         <WrappedComponent classes={classes} {...this.props}/>
      )
    }
  }
}

export default muiInjectSheet
