import grey from 'material-ui/colors/grey'
import red from 'material-ui/colors/red'

export const styles = {
  gridContainer: {
    background: grey[200],
    height: '100%'
  }
}

export const titleHeaderStyles = {
  mainHeader: {
    height: 64,
    backgroundColor: red[400],
    display: 'flex',
    justifyContent: 'space-between',
    padding: [0, 10],
    alignItems: 'center',
    position: 'sticky',
    flexDirection: 'row',
    zIndex: 100,
    top: 0,
    fontFamily: 'Roboto',
    color: 'white'
  },
  searchBar: {
    width: '50%'
  },
  title: {
    fontSize: '2em',
    fontWeight: '100'
  }
}

export const detailsHeaderStyles = {
  detailsHeader: {
    height: 100,
    backgroundColor: red[400],
    padding: [10, 10],
    display: 'flex',
    alignItems: 'flex-end',
    color: 'white'
  }
}
