import React from 'react'
import PlatformListView from 'components/views/PlatformListView'
import state from 'stories/state'
import { action } from '@storybook/react'

const PlatformListViewStory = () => (
  <PlatformListView platforms={state.platforms} onPlatformChanged={action('Platform')}/>
)

export default PlatformListViewStory
