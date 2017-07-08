import React from 'react'
import injectSheet from 'mui-jss-inject'

import SidebarVisibleView from 'components/views/SidebarVisibleView'
import GameGrid from 'components/presentation/game/GameGrid'
import GameCardAdapter from 'components/adapter/GameCardAdapter'

import SearchBar from 'components/presentation/SearchBar'
import PlatformDisplayAdapter from 'components/adapter/PlatformDisplayAdapter'

import grey from 'material-ui/colors/grey'
import red from 'material-ui/colors/red'
const styles = {
  gridContainer: {
    background: grey[200],
    height: '100%'
  }
}

const titleHeaderStyles = {
  mainHeader: {
    height: 64,
    backgroundColor: red[400],
    display: 'flex',
    justifyContent: 'space-between',
    padding: [0, 10],
    alignItems: 'center',
    position: 'sticky',
    flexDirection: 'row',
    zIndex: 100,
    top: 0,
    fontFamily: 'Roboto',
    color: 'white'
  },
  searchBar: {
    width: '50%'
  },
  title: {
    fontSize: '2em',
    fontWeight: '100'
  }
}

const _TitleHeader = ({ classes }) => (
  <div className={classes.mainHeader}>
    <div className={classes.title}>
      Snowflake
    </div>
    <div className={classes.searchBar}>
      <SearchBar />
    </div>
  </div>
)

const TitleHeader = injectSheet(titleHeaderStyles)(_TitleHeader)

/*

import GameCard from 'components/presentation/game/GameCard'
const card = () => (<GameCard
  image="https://upload.wikimedia.org/wikipedia/en/3/32/Super_Mario_World_Coverart.png"
  title="SUper Mario" publisher="Nintendo" landscape />)

const gameList = [...Array(2500)].map(() => card())
*/

const detailsHeaderStyles = {
  detailsHeader: {
    height: 100,
    backgroundColor: red[400],
    padding: [10, 10],
    display: 'flex',
    alignItems: 'flex-end',
    color: 'white'
  }
}

const _DetailsHeader = ({ classes, platform, games }) => (
  <div className={classes.detailsHeader}>
    <PlatformDisplayAdapter platform={platform} gameCount={games.length}/>
  </div>
)

const DetailsHeader = injectSheet(detailsHeaderStyles)(_DetailsHeader)

const GameListView = ({ classes, games, platform }) => (
  <div className={classes.gridContainer}>
    <GameGrid header={[<TitleHeader />, <DetailsHeader platform={platform} games={games}/>]}>
      {
        games.map(g => <GameCardAdapter game={g} />)
      }
    </GameGrid>
  </div>
)

export default injectSheet(styles)(GameListView)
