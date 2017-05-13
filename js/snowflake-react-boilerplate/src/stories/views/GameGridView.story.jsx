import React from 'react'
import injectSheet from 'mui-jss-inject'

import { action } from '@kadira/storybook'
import GameGridView from 'components/views/GameGridView'
import GameCard from 'components/presentation/GameCard'

import { grey } from 'material-ui/styles/colors'



const styles = {
  container: {
    width: '100%',
    backgroundColor: grey[500]
  }
}

const _GameGridViewStory = ({ classes }) => {
  const card = <GameCard
    image="https://upload.wikimedia.org/wikipedia/en/3/32/Super_Mario_World_Coverart.png"
    title="Super Mario World" publisher="Nintendo" />
  return (
    <div className={classes.container}>
      <GameGridView>
        {[...Array(50)].map((x, i) =>
          card
        )}
      </GameGridView>
    </div>)
}

export const GameGridViewStory = injectSheet(styles)(_GameGridViewStory) 