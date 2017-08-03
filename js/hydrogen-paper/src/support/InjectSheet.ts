import * as React from 'react'
import { getDisplayName } from 'recompose'
import { createStyleSheet, withStyles } from 'material-ui/styles'

type ClassList = { [className: string]: string }

/**
 * Exposes the classes props for CSS-in-JS.
 */
export type StyleProps = {
  classes: ClassList
}

type StyleSheetBlock = { [ruleName: string]: string | number | string[] | number[] | (string|number)[] | StyleSheetBlock }

type StyleSheet = { [className: string]: StyleSheetBlock }
type ThemedStyleSheet = (theme: any) => StyleSheet

const sanitize = displayName => (displayName.replace(/(\(|\[)/g, '-').replace(/(\)|\])/g, '')) // todo make this better

/**
 * Injects a plain JSS stylesheet object, or a theme reactor function, into a React Component.
 * Types are inferred correctly if your component is typed with types other than StyleProps, but
 * if your component is otherwise prop-less, you must type the returned function as <{}> or type your props as
 * NoProps & StyleProps.
 * @param styles The JSS stylesheet object or stylesheet function. This should be a plain Javascript object.
 */
const injectSheet = (styles: StyleSheet | ThemedStyleSheet) =>
/**
 * @param component The React component to inject styles into.
 */
 <T extends {}>(component: React.ComponentClass<T & StyleProps> | React.StatelessComponent<T & StyleProps>): React.ComponentClass<T> => {
   const styleSheetName = sanitize(getDisplayName(component))
   const styleSheet = createStyleSheet(styleSheetName, styles)
   return withStyles(styleSheet)(component)
 }

export default injectSheet
