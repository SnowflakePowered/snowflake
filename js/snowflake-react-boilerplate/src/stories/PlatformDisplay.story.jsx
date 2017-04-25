import React from 'react'
import muiInjectSheet from 'utils/muiInjectSheet'
import PlatformDisplay from 'components/presentation/info/PlatformDisplay'

import { red } from 'material-ui/styles/colors'

export const Black = () => (
  <PlatformDisplay
    platformName="Super Nintendo Entertainment System"
    platformAltNamae="スーパーファミコン"
    publisher="Nintendo"
    year="1990" />
)

export const WhiteWithBackground = muiInjectSheet({
  container: {
    color: 'white',
    backgroundColor: red[400],
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
