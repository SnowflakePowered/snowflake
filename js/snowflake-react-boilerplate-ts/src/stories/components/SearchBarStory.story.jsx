import React from 'react'
import injectSheet from 'mui-jss-inject'

import { action } from '@storybook/react'
import SearchBar from 'components/presentation/SearchBar'

export const SearchBarStory = injectSheet({
  container: {
    width: '90%'
  }
})(({ classes }) => (
  <div className={classes.container}>
    <SearchBar tagline="Nintendo Entertainment System" onChange={action('text-changed')} />
  </div>
));

export const SearchBarStoryWithBackground = injectSheet(theme => ({
  container: {
    width: '90%'
  },
  background: {
    width: '100%',
    height: '100%',
    backgroundColor: theme.palette.primary[500],
    display: 'flex',
    alignItems: 'center',
    justifyContent: 'center'
  }

}))(({ classes }) => (
  <div className={classes.background}>
    <div className={classes.container}>
      <SearchBar tagline="Nintendo Entertainment System" onChange={action('text-changed')} />
    </div>
  </div>
))
