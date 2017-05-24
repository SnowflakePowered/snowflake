import React from 'react'
import injectSheet from 'mui-jss-inject'

import Sidebar from 'components/views/Sidebar'

import GameGrid from 'components/presentation/GameGrid'

import GameCardAdapter from 'components/adapter/GameCardAdapter'
import GameCard from 'components/presentation/GameCard'
import SearchBar from 'components/presentation/SearchBar'

const styles = {
  container: {
    width: '100%',
    height: '100%',
    display: 'grid',
    gridTemplateColumns: '[sidebar] 64px [main] auto',
    gridTemplateRows: '',
  },
  sidebarContainer: {
    gridColumnArea: 'sidebar'
  },
  mainContainer: {
    background: 'grey',
    gridColumnStart: 'main',
    height: '100vh'
  },
  mainHeader: {
    height: 64,
    backgroundColor: 'rgba(0,0,0,0.5)',
    width: '100%',
    display: 'flex',
    justifyContent: 'center',
    alignItems: 'center',
    position: 'sticky',
    zIndex: 100,
    top: 0
  },
  searchBar: {
    width: 500
  }
}

const _Header = ({classes}) => (
   <div className={classes.mainHeader}>
     <div className={classes.searchBar}>
       <SearchBar/>
      </div>
   </div>
)

const Header = injectSheet(styles)(_Header)
const card = () => <GameCard
    image="https://upload.wikimedia.org/wikipedia/en/3/32/Super_Mario_World_Coverart.png"
    title="SUper Mario" publisher="Nintendo" landscape/>

const GameListView = ({ classes, games, platform }) => (
  <div className={classes.container}>
    <div className={classes.sidebarContainer}>
      <Sidebar />
    </div>
    <div className={classes.mainContainer}>
        <GameGrid header={<Header/>}>
        {[...Array(2500)].map((x, i) =>
                  card(i + 1)
                )}
        </GameGrid>
    </div>
  </div>
)

export default injectSheet(styles)(GameListView)
