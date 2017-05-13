import React from 'react'
import injectSheet from 'mui-jss-inject'


const styles = {
  container: {
    display: 'flex',
    width: 'inherit',
    height: 'inherit',
    flexDirection: 'column',
    overflowY: 'scroll',
  },
  card: {
    padding: 10,
    display: 'inline-block'
  },
  scrollContainer: {
    overflow: scroll,
    height: '100vh',
    position: 'relative'
  }
}
const GameGridView = ({ classes, children }) => (
  <div className={classes.container}>
    <div className={classes.scrollContainer}>
      {children.map((x, i) =>
        <div className={classes.card}>
          {x}
        </div>)}
    </div>
  </div>
)

export default injectSheet( styles)(GameGridView)