import React from 'react'
import injectSheet from 'react-jss' 
import { Card, CardContent, CardMedia } from 'material-ui/Card'
import { grey } from 'material-ui/styles/colors'

import Text from 'material-ui/Text'

import styleable from 'utils/styleable'
import muiInjectSheet from 'utils/muiInjectSheet'


const styles = {
  cardContainer: {
    maxWidth: 300,
    maxHeight: 500
  },
  cardImage: {
    objectFit: 'cover',
    minWidth: '100%',
    objectPosition: 'center'
  },
  cardImageContainer: {
    maxHeight: 250,
    maxWidth: 300,
    overflow: 'hidden',
    margin: 0,
    position: 'relative'
  },
  cardSubtitle: {
    fontSize: "0.75em",
    color: grey[400]
  }
}

//todo: z-depth on hover
//todo: button on hover

const GameCard = ({classes, image, title, publisher}) => {
  return (
  <div>
      <Card>
        <CardMedia>
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

export default muiInjectSheet(styles)(styleable(GameCard))