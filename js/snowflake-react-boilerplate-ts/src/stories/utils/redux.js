import React from 'react'
import { Provider } from 'react-redux'
import Store from 'store'
import SnowflakeProvider from 'snowflake/SnowflakeProvider'

import { refreshPlatforms } from 'actions/platforms'
import initialState from '../state'

//      this.props.actions.platforms.refreshPlatforms(initialState.platforms)

const StoreInstance = Store({
  platforms: initialState.platforms,
  games: initialState.games,
  state: {},
})

export default () => storyFn => (
  <Provider store={StoreInstance}>
    <SnowflakeProvider>
      {storyFn()}
    </SnowflakeProvider>
  </Provider>
)