import React from 'react'
import injectSheet from 'mui-jss-inject'
import BooleanWidget from 'components/configuration/BooleanWidget'

export const BooleanConfigurationStory = injectSheet({
  container: {
    display: 'block',
    width: '100%'
  },
  card: {
    margin: 10
  }
})(({ classes }) => (
  <div className={classes.container}>
    <BooleanWidget name="Fullscreen" description="Enables exclusive fullscreen mode." />
    <BooleanWidget name="Fullscreen" description="Enables exclusive fullscreen mode." />
    <BooleanWidget name="Fullscreen" description="Enables exclusive fullscreen mode." />
  </div>
))
