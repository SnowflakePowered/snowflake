export const styles = theme => ({
  barContainer: {
    width: '100%',
    display: 'grid',
    gridTemplateColumns: '48px auto 10px',
    alignItems: 'center',
    opacity: 1,
    fontFamily: 'Roboto, sans-serif',
    '&:hover, &:focus': {
      opacity: 1
    }
  },
  searchIcon: {
    justifySelf: 'center'
  },
  textFieldUnderline: {
    '&:after': {
      backgroundColor: theme.palette.background[500]
    }
  }
})
