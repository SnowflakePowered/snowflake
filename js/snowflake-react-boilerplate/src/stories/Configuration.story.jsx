import React from 'react'
import muiInjectSheet from 'utils/muiInjectSheet'
import BooleanWidget from 'components/configuration/BooleanWidget'

export const BooleanConfigurationStory = muiInjectSheet({
  container: {
    display: 'block',
    width: "100%"
  },
  card: {
    margin: 10
  }
})(({classes}) => (
    <div className={classes.container}>
      <BooleanWidget name="Fullscreen" description="Enables exclusive fullscreen mode."/>
      <BooleanWidget name="Fullscreen" description="Enables exclusive fullscreen mode."/>
      <BooleanWidget name="Fullscreen" description="Enables exclusive fullscreen mode."/>
    </div>
));


