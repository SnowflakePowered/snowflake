import React from 'react'
import PlatformListView from 'components/views/PlatformListView'


import withPlatforms from 'snowflake/compose/withPlatforms'
import withState from 'snowflake/compose/withState'
import withActions from 'snowflake/compose/withActions'

import compose from 'recompose/compose'


const PlatformListViewAdapter = ({platforms, state, actions}) => {

  const handlePlatformChanged = p => {
    actions.state.setActivePlatform(p.PlatformID)
  }

  return (
    <PlatformListView platforms={platforms} currentPlatform={state.currentPlatform} onPlatformChanged={handlePlatformChanged}/>
  )
}

export default compose(withPlatforms, withState, withActions)(PlatformListViewAdapter)