import React from 'react'
import Paper from 'material-ui/Paper'
import injectSheet from 'mui-jss-inject'

const styles = {
  imageContainer: {
    display: 'grid',
    gridTemplateColumns: '100%',
    gridTemplateRows: '100%',
    borderRadius: 'inherit',
  },
  image: {
    objectFit: 'cover',
    borderRadius: 'inherit',
    userDrag: 'none',
    userSelect: 'none'
  }
}

const ImageCard = ({classes, image, elevation}) => (
  <Paper elevation={elevation}>
    <div className={classes.imageContainer}>
      <img className={classes.image} src={image}/>
    </div>
  </Paper>
)

export default injectSheet(styles)(ImageCard)