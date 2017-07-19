import React from 'react'
import injectSheet from 'mui-jss-inject'

import Typography from 'material-ui/Typography'
import Card, { CardContent, CardMedia } from 'material-ui/Card'
import grey from 'material-ui/colors/grey'

import GamePlayButton from './GamePlayButton'
import classNames from 'classnames'

import PhotoIcon from 'material-ui-icons/Photo'

import Link from 'components/Link'

export const dimensions = {
  portrait: {
    width: 170,
    height: 200,
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

const styles = {
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
    overflow: 'hidden',
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

// todo: z-depth on hover
// todo: button on hover

const GameCard = ({ classes, image, title, publisher, portrait, landscape, square, guid, platformID }) => (
  <div className={classNames({
    [classes.cardContainer]: true,
    [classes.cardContainerSquare]: square,
    [classes.cardContainerLandscape]: landscape,
    [classes.cardContainerPortrait]: portrait,
    [classes.cardContainerPortrait]: !(portrait && landscape && square)
  })}>
    <Card>
      <CardMedia>
        <div className={classes.playButton}>
          <Link to={`/gamedetail?platform=${platformID}&game=${guid}`}>
            <GamePlayButton />
          </Link>
        </div>
        <div className={classNames({
          [classes.sizer]: true,
          [classes.sizerSquare]: square,
          [classes.sizerLandscape]: landscape,
          [classes.sizerPortrait]: portrait,
          [classes.sizerPortrait]: !(portrait && landscape && square)
        })}>
          <div className={classes.cardImageContainer}>
            {image ? <img className={classes.cardImage} src={image} /> : <PhotoIcon/>}
          </div>
        </div>
      </CardMedia>
      <div className={classes.cardText}>
        <CardContent>
          <Typography type="headline" component="h2" className={classes.cardTitle}>{title}</Typography>
          <Typography component="h3" className={classes.cardSubtitle}>{publisher}</Typography>
        </CardContent>
      </div>
    </Card>
  </div>)

export default injectSheet(styles)(GameCard)