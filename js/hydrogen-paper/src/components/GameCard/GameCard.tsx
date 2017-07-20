import * as React from 'react'
import injectSheet from 'mui-jss-inject'

import Typography from 'material-ui/Typography'
import Card, { CardContent, CardMedia } from 'material-ui/Card'

import GamePlayButton from 'components/GamePlayButton/GamePlayButton'

import PhotoIcon from 'material-ui-icons/Photo'

import Link from 'components/Link/Link'
import * as classNames from 'classnames'
import { styles } from './GameCard.style'

// todo: z-depth on hover
// todo: button on hover
type GameCardProps = {
  // tslint:disable-next-line:no-any
  classes?: any,
  image: string,
  title: string,
  publisher: string,
  portrait?: boolean,
  landscape?: boolean,
  square?: boolean,
  guid: string,
  platformID: string
}

const GameCard: React.SFC<GameCardProps> = ({ classes, image, title, publisher, portrait, landscape, square, guid, platformID }) => (
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
          <Typography type='headline' component='h2' className={classes.cardTitle}>{title}</Typography>
          <Typography component='h3' className={classes.cardSubtitle}>{publisher}</Typography>
        </CardContent>
      </div>
    </Card>
  </div>)

export default injectSheet<GameCardProps>(styles)(GameCard)
