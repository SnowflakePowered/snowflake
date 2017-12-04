import * as React from 'react'
import injectSheet, { StyleProps } from 'support/InjectSheet'
import { styles } from './ImageBackground.style'

type ImageBackgroundProps = {
  image: string
}

const ImageBackground: React.SFC<ImageBackgroundProps & StyleProps> = ({ classes, image }) => (
  <div className={classes.headerBackgroundContainer}>
    <img className={classes.headerBackground}
      src={image} />
  </div>
)

export default injectSheet(styles)(ImageBackground)
