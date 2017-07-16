import React from 'react'
import PlatformDisplay from 'components/presentation/info/PlatformDisplay'

import Typography from 'material-ui/Typography'
const PlatformDisplayAdapter = ({ platform, gameCount }) => {
  if (!platform) {
    return (
      <div>
        <Typography type="subheading">No Platform Selected</Typography>
      </div>
    )
  } else {
    const date = new Date(platform.Metadata.platform_releasedate)
    return (
      <PlatformDisplay
        platformName={platform.FriendlyName}
        publisher={platform.Metadata.platform_company}
        year={date.getFullYear()}
        gameCount = { gameCount }
      />
    )
  }
}

export default PlatformDisplayAdapter
