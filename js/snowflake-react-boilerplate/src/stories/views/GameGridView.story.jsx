import React from 'react'
import injectSheet from 'mui-jss-inject'

import { action } from '@kadira/storybook'
import GameGridView from 'components/views/GameGridView'
import GameCard from 'components/presentation/GameCard'

import { grey } from 'material-ui/styles/colors'



const styles = {
  container: {
    width: '100%',
    height: '100%',
    backgroundColor: grey[100]
  }
}

const _LandscapeGameGridViewStory = ({ classes }) => {
  const card = int => <GameCard
    image="https://upload.wikimedia.org/wikipedia/en/3/32/Super_Mario_World_Coverart.png"
    title={int} publisher="Nintendo" landscape/>
  return (
    <div className={classes.container}>
      <GameGridView landscape>
        {[...Array(250)].map((x, i) =>
          card(i + 1)
        )}
      </GameGridView>
    </div>)
}


const _PortraitGameGridViewStory = ({ classes }) => {
  const card = int => <GameCard
    image="http://vignette2.wikia.nocookie.net/mario/images/6/60/SMBBoxart.png/revision/latest?cb=20120609143443"
    title={int} publisher="Nintendo" portrait/>
  return (
    <div className={classes.container}>
      <GameGridView portrait>
        {[...Array(1000)].map((x, i) =>
          card(i + 1)
        )}
      </GameGridView>
    </div>)
}


const _SquareGameGridViewStory = ({ classes }) => {
  const card = int => <GameCard
    image="https://upload.wikimedia.org/wikipedia/en/d/db/NewSuperMarioBrothers.jpg"
    title={int} publisher="Nintendo" square/>
  return (
    <div className={classes.container}>
      <GameGridView square>
        {[...Array(100)].map((x, i) =>
          card(i + 1)
        )}
      </GameGridView>
    </div>)
}


export const LandscapeGameGridViewStory = injectSheet(styles)(_LandscapeGameGridViewStory) 
export const PortraitGameGridViewStory = injectSheet(styles)(_PortraitGameGridViewStory) 
export const SquareGameGridViewStory = injectSheet(styles)(_SquareGameGridViewStory) 