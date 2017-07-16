import React from 'react'
import injectSheet from 'mui-jss-inject'
import ImageBackground from 'components/presentation/ImageBackground'
import GameLaunchHeaderView from 'components/views/GameLaunchHeaderView'

import Paper from 'material-ui/Paper'
import ImageCard from 'components/presentation/ImageCard'
import Tabs, { Tab } from 'material-ui/Tabs'
import CollapsingParagaph from 'components/presentation/gamedisplay/CollapsingParagraph'

const style = {
  container: {
    display: 'flex',
    overflowX: 'hidden',
    justifyContent: 'center'
  },
  centeredContainer: {
    width: '90%',
    display: 'grid',
    gridTemplateRows: '[gameHeader] minmax(300px, max-content) [main] auto',
    paddingBottom: 10
  },
  gameLaunchHeader: {
    //background: 'white',
    display: 'flex',
    alignItems: 'center',
    margin: [10, 10]
  },
  gameHeader: {
    background: 'rgba(255,255,255,0.9)',
    margin: [10, 0],
    borderRadius: 2,
    gridRow: 'gameHeader',
    display: 'flex',
    flexDirection: 'column'
  },
  gameDescription: {
    margin: [10, 10]
  },
  imageContainer: {
    maxHeight: 250,
    maxWidth: 250,
    height: 250,
    width: 250,
    display: 'flex',
    justifyContent: 'center',
    alignContent: 'center',
    paddingLeft: 20,
    paddingRight: 20
  },
  gameMain: {
    overflowY: 'auto'
  },
  mainPaper: {
    height: 1000
  }
}

/*

        </div>
        */

const GameDisplayView = ({ classes, gameTitle, gamePublisher,
  gameDescription, game }) => (
    <div className={classes.container}>
      <ImageBackground image="https://r.mprd.se/media/images/35787-Super_Mario_World_(USA)-2.jpg" />
      <div className={classes.centeredContainer}>
        <Paper className={classes.gameHeader}>
          <div className={classes.gameLaunchHeader}>
            <div className={classes.imageContainer}>
              <ImageCard
                image="http://vignette2.wikia.nocookie.net/mario/images/6/60/SMBBoxart.png/revision/latest?cb=20120609143443"
              // image="https://upload.wikimedia.org/wikipedia/en/3/32/Super_Mario_World_Coverart.png"
              />
            </div>
            <div className={classes.gameTitle}>
              <GameLaunchHeaderView
                gameTitle={gameTitle}
                gamePublisher={gamePublisher}
              />
            </div>
          </div>
          <div className={classes.gameDescription}>
            <CollapsingParagaph>
            "Lorem ipsum dolor sit amet,
              consectetur adipiscing elit,
                sed do eiusmod tempor incididunt ut
                labore et dolore magna aliqua.
                Ut enim ad minim veniam, quis nostrud
                exercitation ullamco laboris nisi ut
                aliquip ex ea commodo consequat.
                Duis aute irure dolor in reprehenderit
                in voluptate velit esse cillum dolore
                eu fugiat nulla pariatur. Excepteur sint
                occaecat cupidatat non proident, sunt in
                culpa qui officia deserunt mollit
                anim id est laborum."
                "Lorem ipsum dolor sit amet,
              consectetur adipiscing elit,
                sed do eiusmod tempor incididunt ut
                labore et dolore magna aliqua.
                Ut enim ad minim veniam, quis nostrud
                exercitation ullamco laboris nisi ut
                aliquip ex ea commodo consequat.
                Duis aute irure dolor in reprehenderit
                in voluptate velit esse cillum dolore
                eu fugiat nulla pariatur. Excepteur sint
                occaecat cupidatat non proident, sunt in
                culpa qui officia deserunt mollit
                anim id est laborum."
                "Lorem ipsum dolor sit amet,
              consectetur adipiscing elit,
                sed do eiusmod tempor incididunt ut
                labore et dolore magna aliqua.
                Ut enim ad minim veniam, quis nostrud
                exercitation ullamco laboris nisi ut
                aliquip ex ea commodo consequat.
                Duis aute irure dolor in reprehenderit
                in voluptate velit esse cillum dolore
                eu fugiat nulla pariatur. Excepteur sint
                occaecat cupidatat non proident, sunt in
                culpa qui officia deserunt mollit
                anim id est laborum."
              </CollapsingParagaph>
          </div>
        </Paper>

        <Paper className={classes.mainPaper}>
          <GameDisplayViewMenu />
        </Paper>
      </div>
    </div>
  )

class GameDisplayViewMenu extends React.Component {
  state = {
    index: 0
  }

  handleChange = (event, index) => {
    this.setState({ index });
  };

  render() {
    const classes = this.props.classes;

    return (
      <Tabs
        index={this.state.index}
        onChange={this.handleChange}
        indicatorColor="primary"
        textColor="primary"
        centered
      >
        <Tab label="Item One" />
        <Tab label="Item Two" />
        <Tab label="Item Three" />
      </Tabs>
    );
  }
}

export default injectSheet(style)(GameDisplayView)
