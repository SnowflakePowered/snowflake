import React from 'react'
import injectSheet from 'react-jss'
import GameCard from '../components/presentation/GameCard'

const GameCardStory = injectSheet({
  container: {
    display: 'flex'
  },
  card: {
    margin: 10
  }
})(({classes}) => (
    <div className={classes.container}>
      <GameCard className={classes.card} image="https://upload.wikimedia.org/wikipedia/en/3/32/Super_Mario_World_Coverart.png" 
                                         title="Super Mario World" publisher="Nintendo"/>
      <GameCard className={classes.card} image="http://vignette2.wikia.nocookie.net/mario/images/6/60/SMBBoxart.png/revision/latest?cb=20120609143443" 
                                         title="Super Mario Bros." publisher="Nintendo"/>
    </div>
));

export const Single =  () => <GameCard image="http://vignette2.wikia.nocookie.net/mario/images/6/60/SMBBoxart.png/revision/latest?cb=20120609143443" 
                                         title="Super Mario Bros." publisher="Nintendo"/>
export default GameCardStory;
