import * as React from 'react'
import injectSheet from 'mui-jss-inject'

import GameGrid from 'components/GameCardGrid/GameCardGrid'
import GameCard from 'containers/GameCard/GameCardContainer'
import SearchBar from 'components/SearchBar/SearchBar'

import PlatformDisplay from 'containers/PlatformDisplay/PlatformDisplayContainer'

import { Platform, Game } from 'snowflake-remoting'
import { detailsHeaderStyles, titleHeaderStyles, styles } from './GameList.style'

const _TitleHeader = ({ classes }: { classes?: any }) => (
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

type GameListViewProps = {
  classes?: any,
  platform: Platform,
  games: Game[]
}
const _DetailsHeader: React.SFC<GameListViewProps> = ({ classes, platform, games }) => (
  <div className={classes.detailsHeader}>
    <PlatformDisplay platform={platform} gameCount={games.length}/>
  </div>
)

const DetailsHeader = injectSheet(detailsHeaderStyles)(_DetailsHeader)

const GameListView: React.SFC<GameListViewProps> = ({ classes, games, platform }) => (
  <div className={classes.gridContainer}>
    <GameGrid header={[<TitleHeader />, <DetailsHeader platform={platform} games={games}/>]}>
      {
        games.map(g => <GameCard game={g} />)
      }
    </GameGrid>
  </div>
)

export default injectSheet<GameListViewProps>(styles)(GameListView)
