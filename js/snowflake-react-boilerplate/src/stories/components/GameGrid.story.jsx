import React from 'react'
import injectSheet from 'mui-jss-inject'

import GameGrid from 'components/presentation/game/GameGrid'
import GameCard from 'components/presentation/game/GameCard'

import grey from 'material-ui/colors/grey'

const styles = {
  container: {
    width: '100%',
    height: '100%',
    backgroundColor: grey[100]
  }
}


const _LandscapeGameGridViewStory = ({ classes }) => {
  const card = int => (<GameCard 
        key={int}
        image="https://upload.wikimedia.org/wikipedia/en/3/32/Super_Mario_World_Coverart.png" 
        title={int} publisher="Nintendo" landscape/>)
  const gameList = [...Array(2500)].map((x, i) => card(i + 1))

  return (
    <div className={classes.container}>
      <GameGrid landscape>
        {
          gameList
        }
      </GameGrid>
    </div>)
}


const _PortraitGameGridViewStory = ({ classes }) => {
  const card = int => (<GameCard
    key={int}
    image="http://vignette2.wikia.nocookie.net/mario/images/6/60/SMBBoxart.png/revision/latest?cb=20120609143443"
    title={int} publisher="Nintendo" portrait/>)
  const gameList = [...Array(1000)].map((x, i) => card(i + 1))
  return (
    <div className={classes.container}>
      <GameGrid portrait>
        {
          gameList
        }
      </GameGrid>
    </div>)
}


const _SquareGameGridViewStory = ({ classes }) => {
  const card = int => (<GameCard
    key={int}
    image="https://upload.wikimedia.org/wikipedia/en/d/db/NewSuperMarioBrothers.jpg"
    title={int} publisher="Nintendo" square/>)
  const gameList = [...Array(100)].map((x, i) => card(i + 1))
  return (
    <div className={classes.container}>
      <GameGrid square>
        {
          gameList
        }
      </GameGrid>
    </div>)
}


export const LandscapeGameGridViewStory = injectSheet(styles)(_LandscapeGameGridViewStory) 
export const PortraitGameGridViewStory = injectSheet(styles)(_PortraitGameGridViewStory) 
export const SquareGameGridViewStory = injectSheet(styles)(_SquareGameGridViewStory) 