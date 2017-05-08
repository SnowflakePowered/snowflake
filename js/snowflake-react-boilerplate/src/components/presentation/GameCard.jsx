import React from 'react'
import injectSheet from 'mui-jss-inject'

import Text from 'material-ui/Text'
import { Card, CardContent, CardMedia } from 'material-ui/Card'
import { grey } from 'material-ui/styles/colors'

import GamePlayButton from './GamePlayButton'
import styleable from 'utils/styleable'


const styles = {
  cardContainer: {
    maxWidth: 250,
    maxHeight: 250
  },
  cardImage: {
    objectFit: 'cover',
    objectPosition: 'center',
    userDrag: 'none',
    maxHeight: 'inherit',
    minWidth: '100%'
  },

  cardImageContainer: {
    maxHeight: 250,
    maxWidth: 300,
    overflow: 'hidden',
    margin: 0,
    position: 'relative'
  },
  cardSubtitle: {
    fontSize: '0.75em',
    color: grey[400]
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
  }
}

// todo: z-depth on hover
// todo: button on hover

const GameCard = ({ classes, image, title, publisher }) => {
  return (
    <div className={classes.cardContainer}>
      <Card>
        <CardMedia>
          <div className={classes.playButton}>
            <GamePlayButton />
          </div>
          <div className={classes.cardImageContainer}>
            <img className={classes.cardImage} src={image} />
          </div>
        </CardMedia>
        <CardContent>
          <Text type="headline" component="h2">{title}</Text>
          <Text component="h3" className={classes.cardSubtitle}>{publisher}</Text>
        </CardContent>
      </Card>
    </div>)
}

export default injectSheet(styles)(styleable(GameCard))