import React from 'react'
import injectSheet from 'react-jss'
import PlatformDisplay from 'components/presentation/info/PlatformDisplay'

import { red500 } from 'material-ui/styles/colors'

export const Black = () => (
  <PlatformDisplay
    platformName="Super Nintendo Entertainment System"
    platformAltNamae="スーパーファミコン"
    publisher="Nintendo"
    year="1990" />
)

export const WhiteWithBackground = injectSheet({
  container: {
    color: 'white',
    backgroundColor: red500,
    height: '100%'
  },
  infoDisplay: {
    height: '100%'
  }
})(({ classes }) => (
  <div className={classes.container}>
    <PlatformDisplay
      className={classes.infoDisplay}
      platformName="Super Nintendo Entertainment System"
      platformAltName="スーパーファミコン"
      publisher="Nintendo"
      year="1990" />
  </div>
));
