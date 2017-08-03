export const styles = {
  container: {
    width: '100%',
    height: '100%',
    maxHeight: '100%',
    maxWidth: '100%',
    display: 'grid',
    gridTemplateColumns: '64px auto',
    gridTemplateRows: '',
    gridTemplateAreas: [
      '"sidebar main"'
    ]
  },
  sidebarContainer: {
    gridArea: 'sidebar'
  },
  mainContainer: {
    gridArea: 'main',
    height: '100vh'
  }
}
