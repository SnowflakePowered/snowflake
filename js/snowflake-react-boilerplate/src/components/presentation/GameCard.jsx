import React from 'react';
import injectSheet from 'react-jss' 
import {Card, CardActions, CardHeader, CardMedia, CardTitle, CardText} from 'material-ui/Card';
import FlatButton from 'material-ui/FlatButton';
import styleable from '../../utils/styleable'

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
  }
  
}

//todo: z-depth on hover
//todo: button on hover
const GameCard = ({sheets, classes, image, title, publisher}) => (
  <div className={classes.cardContainer}>
      <Card>
        <CardMedia>
          <div className={classes.cardImageContainer}>
              <img className={classes.cardImage} src={image} />
          </div>
        </CardMedia>
        <CardTitle title={title} subtitle={publisher} />
      </Card>
  </div>
);

export default injectSheet(styles)(styleable(GameCard));