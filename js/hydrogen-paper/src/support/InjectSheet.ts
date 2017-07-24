import * as React from 'react'
import { getDisplayName } from 'recompose'
import { createStyleSheet, withStyles } from 'material-ui/styles'

type ClassList = { [className: string]: string }

export type StyleProps = {
  classes: ClassList
}

type StyleSheetBlock = { [ruleName: string]: string | number | string[] | number[] | (string|number)[] | StyleSheetBlock }

type StyleSheet = { [className: string]: StyleSheetBlock }
type ThemedStyleSheet = (theme: any) => StyleSheet

const sanitize = displayName => (displayName.replace(/(\(|\[)/g, '-').replace(/(\)|\])/g, '')) // todo make this better

const injectSheet = <T>(styles: StyleSheet | ThemedStyleSheet) =>
 (component: React.ComponentClass<T> | React.StatelessComponent<T>): React.ComponentClass<T> => {
   const styleSheetName = sanitize(getDisplayName(component))
   const styleSheet = createStyleSheet(styleSheetName, styles)
   return withStyles(styleSheet)(component)
 }

export default injectSheet
