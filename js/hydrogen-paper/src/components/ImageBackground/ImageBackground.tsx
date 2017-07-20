import * as React from 'react'
import injectSheet from 'mui-jss-inject'
import { styles } from './ImageBackground.style'

type ImageBackgroundProps = {
  classes?: any,
  image: string
}

const ImageBackground: React.SFC<ImageBackgroundProps> = ({ classes, image }) => (
  <div className={classes.headerBackgroundContainer}>
    <img className={classes.headerBackground}
      src={image} />
  </div>
)

export default injectSheet<ImageBackgroundProps>(styles)(ImageBackground)
