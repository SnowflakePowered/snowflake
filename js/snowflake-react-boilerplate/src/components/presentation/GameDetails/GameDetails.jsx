import React from 'react';
import injectSheet from 'react-jss'
import { Card, CardActions, CardHeader, CardMedia, CardTitle, CardText } from 'material-ui/Card';
import FlatButton from 'material-ui/FlatButton';
import styleable from 'utils/styleable'
import GameDisplay from 'components/presentation/info/GameDisplay'
import GameDetailsTabs from './GameDetailsTabs'
import GameDetailsEmulatorSelect from './GameDetailsEmulatorSelect'
import FloatingActionButton from 'material-ui/FloatingActionButton';
import PlayArrow from 'material-ui/svg-icons/av/play-arrow';


const styles = {
  cardContainer: {
    width: '100%'
  },
  cardImage: {
    minWidth: '100%',
    objectPosition: '50% 50%',
    objectFit: 'cover'
  },
  cardImageContainer: {
    maxHeight: 250,
    maxWidth: 500,
    overflow: 'hidden',
    margin: 0,
    position: 'relative'
  },
  headerArea: {
    display: 'grid',
    gridTemplateColumns: "60% 40%"
  }
  
}

//todo: z-depth on hover
//todo: button on hover
/*const GameDetails = ({ sheets, classes, image, title, publisher, year, summary, emulators, selectedEmulator }) => (
  <div className={classes.cardContainer}>
    <Card>
      <CardMedia>
        <div className={classes.cardImageContainer}>
          <img className={classes.cardImage} src={image} />
        </div>
      </CardMedia>
      <CardTitle>
        
        <div className={classes.headerArea}>
         <GameDisplay gameTitle={title} publisher={publisher} year={year} />
         <GameDetailsEmulatorSelect emulators={emulators} selectedEmulator={selectedEmulator}/>
        </div>
      </CardTitle>
      <CardText>
        {summary}
      </CardText>
      <GameDetailsTabs/>
    </Card>
  </div>
);*/

const GameDetails = ({ sheets, classes, image, title, publisher, year, summary, emulators, selectedEmulator }) => (
  <div className={classes.cardContainer}>
    <Card>
      <CardMedia>
        <div className={classes.cardImageContainer}>
          <img className={classes.cardImage} src={image} />
        </div>
      </CardMedia>
      <CardTitle>
        
        <div className={classes.headerArea}>
         <GameDisplay gameTitle={title} publisher={publisher} year={year} />
         <GameDetailsEmulatorSelect emulators={emulators} selectedEmulator={selectedEmulator}/>
        </div>
      </CardTitle>
      <CardText>
        {summary}
      </CardText>
      <GameDetailsTabs/>
    </Card>
  </div>
);

export default injectSheet(styles)(styleable(GameDetails))