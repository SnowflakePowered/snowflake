import React from 'react'
import PlatformListView from 'components/views/PlatformListView'

import withPlatforms from 'snowflake/compose/withPlatforms'
import withState from 'snowflake/compose/withState'
import withActions from 'snowflake/compose/withActions'
import compose from 'recompose/compose'

const PlatformListViewAdapter = ({platforms, matchPlatform, actions}) => {
  return (
    <PlatformListView platforms={platforms} currentPlatform={matchPlatform} />
  )
}

export default compose(withPlatforms, withActions)(PlatformListViewAdapter)
