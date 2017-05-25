import React from 'react'
import injectSheet from 'react-jss'
import GameCard from 'components/presentation/game/GameCard'

export const MultiCard = injectSheet({
  container: {
    display: 'flex'
  },
  card: {
    margin: 10
  }
})(({ classes }) => (
  <div className={classes.container}>
    <div className={classes.card}>
      <GameCard
        horizontal
        image="https://upload.wikimedia.org/wikipedia/en/3/32/Super_Mario_World_Coverart.png"
        title="Super Mario World ELIPSIS TESTING" publisher="Nintendo FROM Aintendo FROM Aintendo FROM Aintendo FROM Aintendo FROM Aintendo FROM Aintendo FROM ANOTHER WORLD" landscape/>
    </div>
    <div className={classes.card}>
      <GameCard image="http://vignette2.wikia.nocookie.net/mario/images/6/60/SMBBoxart.png/revision/latest?cb=20120609143443"
        title="Super Mario Bros. Chou Nagai Monji Gaiden NEW" publisher="Nintendo" />
    </div>
    <div className={classes.card}>
      <GameCard image="https://upload.wikimedia.org/wikipedia/en/d/db/NewSuperMarioBrothers.jpg"
        title="Square New Super Mario Bros. Chou Nagai Monji Gaiden NEW" publisher="Nintendo" square/>
    </div>
  </div>
))

export const SingleCard = () => <GameCard
  image="http://vignette2.wikia.nocookie.net/mario/images/6/60/SMBBoxart.png/revision/latest?cb=20120609143443"
  title="Super Mario Bros." publisher="Nintendo" />
