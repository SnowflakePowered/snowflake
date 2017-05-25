import React from 'react'
import injectSheet from 'mui-jss-inject'

import Sidebar from 'components/views/Sidebar'

import GameGrid from 'components/presentation/GameGrid'

import GameCardAdapter from 'components/adapter/GameCardAdapter'
import GameCard from 'components/presentation/GameCard'

import SearchBar from 'components/presentation/SearchBar'
import PlatformDisplayAdapter from 'components/adapter/PlatformDisplayAdapter'

import { grey, red } from 'material-ui/styles/colors'

import withGames from 'snowflake/compose/withGames'

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
    background: grey[200],
    gridColumnStart: 'main',
    height: '100vh'
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
    color: 'white',
  },
  searchBar: {
    width: '50%'
  },
  title: {
    fontSize: '2em',
    fontWeight: '100'
  }
}
const _TitleHeader = ({classes}) => (
   <div className={classes.mainHeader}>
    <div className={classes.title}>
         Snowflake
    </div>
     <div className={classes.searchBar}>
       <SearchBar/>
      </div>
   </div>
)

const TitleHeader = injectSheet(titleHeaderStyles)(_TitleHeader)
const card = () => <GameCard
    image="https://upload.wikimedia.org/wikipedia/en/3/32/Super_Mario_World_Coverart.png"
    title="SUper Mario" publisher="Nintendo" landscape/>


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

const _DetailsHeader = ({classes, platform}) => (
  <div className={classes.detailsHeader}>
    <PlatformDisplayAdapter platform={platform}/>
  </div>
)

const DetailsHeader = injectSheet(detailsHeaderStyles)(_DetailsHeader)

const GameListView = ({ classes, games, platform }) => (
  <div className={classes.container}>
    <div className={classes.sidebarContainer}>
      <Sidebar />
    </div>
    <div className={classes.mainContainer}>
        <GameGrid header={[<TitleHeader/>, <DetailsHeader platform={platform}/>]}>
        {
          games.map(g => <GameCardAdapter game={g}/>)
        }
        </GameGrid>
    </div>
  </div>
)

export default withGames(injectSheet(styles)(GameListView))
