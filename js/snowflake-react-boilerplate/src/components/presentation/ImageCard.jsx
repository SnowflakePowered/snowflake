import React from 'react'
import PropTypes from 'prop-types'

import Paper from 'material-ui/Paper'
import injectSheet from 'mui-jss-inject'

const styles = {
  imageContainer: {
    display: 'grid',
    gridTemplateColumns: '100%',
    gridTemplateRows: '100%',
    borderRadius: 'inherit'
  },
  image: {
    objectFit: 'cover',
    borderRadius: 'inherit',
    userDrag: 'none',
    userSelect: 'none'
  },
  paperContainer: {
    display: 'block'
  },
  paper: {
    display: 'inline-block'
  }
}

const ImageCard = ({ classes, image, elevation }) => (
  <div className={classes.paperContainer}>
    <Paper elevation={elevation} className={classes.paper}>
      <div className={classes.imageContainer}>
        <img className={classes.image} src={image} />
      </div>
    </Paper>
  </div>
)

ImageCard.propTypes = {
  image: PropTypes.string.isRequired,
  elevation: PropTypes.number,
  classes: PropTypes.object
}
export default injectSheet(styles)(ImageCard)
