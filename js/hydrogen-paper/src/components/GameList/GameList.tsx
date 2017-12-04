import * as React from 'react'
import injectSheet, { StyleProps } from 'support/InjectSheet'
import { NoProps } from 'support/NoProps'

import GameGrid from 'components/GameCardGrid/GameCardGrid'
import GameCard from 'containers/GameCard/GameCardContainer'
import SearchBar from 'components/SearchBar/SearchBar'

import PlatformDisplay from 'containers/PlatformDisplay/PlatformDisplayContainer'

import { Platform, Game } from 'snowflake-remoting'
import { detailsHeaderStyles, titleHeaderStyles, styles } from './GameList.style'

const _TitleHeader: React.SFC<NoProps & StyleProps> = ({ classes }) => (
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
  platform: Platform,
  games: Game[]
}

const _DetailsHeader: React.SFC<GameListViewProps & StyleProps> = ({ classes, platform, games }) => (
  <div className={classes.detailsHeader}>
    <PlatformDisplay platform={platform} gameCount={games.length}/>
  </div>
)

const DetailsHeader = injectSheet(detailsHeaderStyles)(_DetailsHeader)

const GameListView: React.SFC<GameListViewProps & StyleProps> = ({ classes, games, platform }) => (
  <div className={classes.gridContainer}>
    <GameGrid header={[<TitleHeader />, <DetailsHeader platform={platform} games={games}/>]}>
      {
        games.map(g => <GameCard game={g} key={g.Guid}/>)
      }
    </GameGrid>
  </div>
)

export default injectSheet(styles)(GameListView)
