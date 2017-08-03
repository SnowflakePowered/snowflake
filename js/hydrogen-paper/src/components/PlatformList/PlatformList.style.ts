import grey from 'material-ui/colors/grey'

export const styles = {
  platformSelector: {
    overflowY: 'auto',
    overflowX: 'hidden',
    gridArea: 'selector',
    zIndex: 2
  },
  container: {
    display: 'grid',
    gridTemplateColumns: '30% auto',
    gridTemplateAreas: [
      '"selector info"'
    ],
    height: '100%'
  },
  platformDisplay: {
    display: 'grid',
    gridTemplateRows: '60% auto',
    gridTemplateAreas: '"image" "info"'
  },
  platformImage: {
    backgroundColor: grey[200],
    gridArea: 'image'
  },
  platformInformation: {
    gridArea: 'info'
  },
  platformInformationInner: {
    display: 'grid',
    gridTemplateRows: '80% 20%',
    gridTemplateAreas: '"info" "menu"',
    height: '100%',
    width: '100%'
  },
  menu: {
    gridArea: 'menu',
    display: 'flex',
    alignItems: 'center',
    justifyContent: 'flex-end'

  },
  platformInfoDisplay: {
    gridArea: 'info'
  }
}
