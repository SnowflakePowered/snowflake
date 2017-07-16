import React from 'react'
import PlatformListViewAdapter from 'components/adapter/PlatformListViewAdapter'
import state from 'stories/state'
import { action } from '@storybook/react'

const PlatformListViewStory = () => (
  <PlatformListViewAdapter/>
)
// platforms={state.platforms} onPlatformChanged={action('Platform')}
export default PlatformListViewStory
