import * as React from 'react'
import PlatformDisplay from 'components/PlatformDisplay/PlatformDisplay'

import Typography from 'material-ui/Typography'
import { Platform } from 'snowflake-remoting'

type PlatformDisplayContainerProps = {
  platform: Platform,
  gameCount: number
}

const PlatformDisplayContainer: React.SFC<PlatformDisplayContainerProps> = ({ platform, gameCount }: PlatformDisplayContainerProps) => {
  if (!platform) {
    return (
      <div>
        <Typography type='subheading'>No Platform Selected</Typography>
      </div>
    )
  } else {
    const date = new Date(platform.Metadata.platform_releasedate)
    return (
      <PlatformDisplay
        platformName={ platform.FriendlyName }
        publisher={ platform.Metadata.platform_company }
        year={ date.getFullYear().toString() }
        gameCount = { gameCount }
      />
    )
  }
}

export default PlatformDisplayContainer
