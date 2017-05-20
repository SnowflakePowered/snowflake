import React from 'react'
import MuiThemeProvider, { MUI_SHEET_ORDER } from 'material-ui/styles/MuiThemeProvider'
import createPalette from 'material-ui/styles/palette'
import createMuiTheme from 'material-ui/styles/theme'

let styleManager

export default ({ primary, accent, type = 'light' }) => ({props}) => {
  const palette = createPalette({
    primary: primary,
    accent: accent,
    type: 'light'
  })

  const theme = createMuiTheme({ palette })

  if (!styleManager) {
    const themeContext = MuiThemeProvider.createDefaultContext({ theme })
    styleManager = themeContext.styleManager
  } else {
    styleManager.updateTheme(theme)
  }

  console.log(styleManager)

  styleManager.setSheetOrder(MUI_SHEET_ORDER.concat([
    'AppContent',
    'AppDrawer',
    'AppDrawerNavItem',
    'AppFrame',
    'MarkdownDocs',
    'MarkdownElement',
    'Demo'
  ]))

  return (
    <MuiThemeProvider theme={theme} styleManager={styleManager}>
      {props.children}
    </MuiThemeProvider>
  )
}
