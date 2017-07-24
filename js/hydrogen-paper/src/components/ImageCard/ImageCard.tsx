import * as React from 'react'
import * as PropTypes from 'prop-types'

import Paper from 'material-ui/Paper'
import injectSheet from 'support/InjectSheet'

const styles = {
  imageContainer: {
    borderRadius: 'inherit',
    maxWidth: 'inherit',
    maxHeight: 'inherit',
    display: 'grid'
  },
  image: {
    objectFit: 'contain',
    borderRadius: 'inherit',
    userDrag: 'none',
    userSelect: 'none',
    maxHeight: 'inherit',
    maxWidth: 'inherit'
  },
  paperContainer: {
    display: 'block',
    maxWidth: 'fit-content',
    maxHeight: '-webkit-fill-available'
  },
  paper: {
    display: 'inline-block',
    maxWidth: 'inherit',
    maxHeight: 'inherit'
  },
  container: {
    width: 'inherit',
    height: 'inherit',
    display: 'flex',
    alignItems: 'center',
    justifyContent: 'center'
  },
  padding: {
    width: 'inherit',
    height: 'inherit'
  }
}

type ImageCardProps = {
  // tslint:disable-next-line:no-any
  classes?: any
  image: string,
  elevation?: number
}

const ImageCard: React.SFC<ImageCardProps> = ({ classes, image, elevation }) => (
  <div className={classes.padding}>
    <div className={classes.container}>
      <div className={classes.paperContainer}>
        <Paper elevation={elevation || 1} className={classes.paper}>
          <div className={classes.imageContainer}>
            <img className={classes.image} src={image} />
          </div>
        </Paper>
      </div>
    </div>
  </div>
)

ImageCard.propTypes = {
  image: PropTypes.string.isRequired,
  elevation: PropTypes.number
}

export default injectSheet<ImageCardProps>(styles)(ImageCard)
