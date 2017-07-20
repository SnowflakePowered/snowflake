import grey from 'material-ui/colors/grey'

export const dimensions = {
  portrait: {
    width: 170,
    height: 200
  },
  landscape: {
    width: 185,
    height: 135
  },
  square: {
    width: 175,
    height: 165
  },
  contentHeight: 84
}

export const styles = {
  cardContainer: {
    display: 'inline-block'
  },

  cardContainerPortrait: {
    width: dimensions.portrait.width
  },
  cardContainerLandscape: {
    width: dimensions.landscape.width
  },
  cardContainerSquare: {
    width: dimensions.square.width
  },

  cardImage: {
    objectFit: 'cover',
    objectPosition: 'center',
    userDrag: 'none',
    maxHeight: 'inherit',
    height: '100%',
    width: '100%'
  },

  cardImageContainer: {
    overflow: 'hidden',
    margin: 0,
    position: 'relative',
    display: 'flex',
    width: '100%',
    height: '100%',
    justifyContent: 'center',
    alignItems: 'center',
    backgroundColor: grey[100]
  },

  sizerPortrait: {
    height: dimensions.portrait.height
  },
  sizerLandscape: {
    height: dimensions.landscape.height
  },
  sizerSquare: {
    height: dimensions.square.height
  },
  sizer: {
    width: '100%',
    display: 'flex',
    justifyContent: 'center',
    alignItems: 'center',
    backgroundColor: grey[300]
  },
  cardSubtitle: {
    fontSize: '0.75em',
    color: grey[400],
    whiteSpace: 'nowrap',
    textOverflow: 'ellipsis',
    overflow: 'hidden'
  },
  cardTitle: {
    whiteSpace: 'nowrap',
    textOverflow: 'ellipsis',
    overflow: 'hidden',
    fontSize: '0.9em'
  },
  playButton: {
    position: 'absolute',
    bottom: 0,
    right: 0,
    marginRight: 10,
    marginBottom: 10,
    opacity: 0,
    userSelect: 'none',
    transition: 'opacity 0.2s ease',
    zIndex: 1,
    '$cardContainer:hover &': {
      opacity: 1,
      userSelect: 'auto'
    }
  },
  cardText: {
    height: dimensions.contentHeight
  }
}
