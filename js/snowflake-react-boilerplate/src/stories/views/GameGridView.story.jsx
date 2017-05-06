import React from 'react'
import injectSheet from 'mui-jss-inject'

import { action } from '@kadira/storybook'
import GameGridView from 'components/views/GameGridView'
import GameCard from 'components/presentation/GameCard'

import { grey } from 'material-ui/styles/colors'

const card = <GameCard
      image="https://upload.wikimedia.org/wikipedia/en/3/32/Super_Mario_World_Coverart.png"
      title="Super Mario World" publisher="Nintendo" />

export const GameGridViewStory = injectSheet({
  container: {
    width: '100%',
    height: '100%',
    backgroundColor: grey[500]
  }
})(({ classes }) => (
  <div className={classes.container}>
    <GameGridView>
      {card}
    </GameGridView>
  </div>
));
