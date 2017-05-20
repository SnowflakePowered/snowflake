import React from 'react'
import injectSheet from 'mui-jss-inject'

import Sidebar from 'components/views/Sidebar'

import GameGrid from 'components/presentation/GameGrid'

import GameCardContainer from 'components/container/GameCardContainer'

const styles = {
  container: {
    width: '100%',
    height: '100%',
    display: 'grid',
    gridTemplateColumns: '[sidebar] 64px [main] auto',
    gridTemplateRows: ''
  },
  sidebarContainer: {
    gridColumnArea: 'sidebar'
  },
  mainContainer: {
    background: 'grey',
    gridColumnArea: 'main'
  }
}

const GameListView = ({ classes, games, platform }) => (
  <div className={classes.container}>
    <div className={classes.sidebarContainer}>
      <Sidebar />
    </div>
    <div className={classes.mainContainer}>
      <GameGrid>
        {
          games.map(g => <)
        }
      </GameGrid>
    </div>
  </div>
)

export default injectSheet(styles)(GameListView)
