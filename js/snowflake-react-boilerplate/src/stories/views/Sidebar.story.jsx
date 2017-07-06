import React from 'react'
import injectSheet from 'mui-jss-inject'

import { action } from '@storybook/react'
import Sidebar from 'components/views/Sidebar'

import grey from 'material-ui/colors/grey'

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
