import React from 'react'
import injectSheet from 'mui-jss-inject'

import { action } from '@kadira/storybook'
import Sidebar from 'components/presentation/Sidebar'

import { grey } from 'material-ui/styles/colors'

export const SidebarStory = injectSheet({
  container: {
    width: '100%',
    height: '100%',
    backgroundColor: grey[500]
  }
})(({ classes }) => (
  <div className={classes.container}>
    <Sidebar/>
  </div>
));
