import React from 'react'
import injectSheet from 'mui-jss-inject'
import { AutoSizer, Grid, ColumnSizer, WindowScroller } from 'react-virtualized'
import { dimensions } from 'components/presentation/GameCard'

const styles = {
  container: {
    display: 'flex',
    width: '100%',
    height: '100%',
    flexDirection: 'column',
    overflowY: 'scroll',
    '-webkit-select': 'none'
  },
  card: {
    padding: 10,
    display: 'inline-block'
  },
  autoSizerContainer: {
    height: '100%'
  },
  cellWrapper: {
    display: 'flex',
    justifyContent: 'center',
    alignItems: 'center'
  },
  gridContainer: {
    display: 'flex',
    justifyContent: 'center'
  }
}

const cellRenderer = ({ classes, children, numberOfRows, numberOfColumns }) => ({ columnIndex, key, rowIndex, style }) => {
  return (
    <div key={key}
      style={style}
      className={classes.cellWrapper}
    >
      {children[rowIndex * numberOfColumns + columnIndex]}
    </div>
  )
}

const padding = 24
const getDimensions = (portrait, landscape, square) => {
  let dimensionObject;
  if (portrait) {
    dimensionObject = dimensions.portrait
  } else if (landscape) {
    dimensionObject = dimensions.landscape
  } else if (square) {
    dimensionObject = dimensions.square
  } else {
    dimensionObject = dimensions.portrait
  }

  return { BOX_HEIGHT: dimensionObject.height + dimensions.contentHeight + padding, BOX_WIDTH: dimensionObject.width + padding }
}

class GameGridView extends React.Component {
  constructor(props) {
    super(props)
    this.state = {
      scrollTop: 0
    }
  }

  /**
   * @param {Number} scrollTop 
   */
  updatePosition(scrollTop) {
    requestAnimationFrame(() => this.setState({ scrollTop: scrollTop }))
  }

  render() {
    const { BOX_HEIGHT, BOX_WIDTH } = getDimensions(this.props.portrait, this.props.landscape, this.props.square)
    return (
      <div className={this.props.classes.container} onScroll={(e) => this.updatePosition(e.target.scrollTop)}>
        <div className={this.props.classes.autoSizerContainer}>
          <AutoSizer>
            {({ height, width }) => {
              const numberOfColumns = Math.floor(width / BOX_WIDTH)
              const CENTERED_BOX_WIDTH = BOX_WIDTH + (BOX_WIDTH / numberOfColumns / 2)
              const numberOfRows = Math.ceil(this.props.children.length / numberOfColumns)
              return (
                <ColumnSizer
                  columnMaxWidth={CENTERED_BOX_WIDTH}
                  columnMinWidth={BOX_WIDTH}
                  columnCount={numberOfColumns}
                  width={width}>
                  {({ adjustedWidth, getColumnWidth, registerChild }) => (
                    <div className={this.props.classes.gridContainer} style={{ width: width || 0 }}>
                      <Grid
                        style={{ outline: 'none', userSelect: 'none' }}
                        ref={registerChild}
                        cellRenderer={cellRenderer({ classes: this.props.classes, children: this.props.children, numberOfRows, numberOfColumns })}
                        columnCount={numberOfColumns}
                        rowCount={numberOfRows}
                        columnWidth={getColumnWidth}
                        rowHeight={BOX_HEIGHT}
                        overscanRowCount={5}
                        height={height}
                        width={adjustedWidth}
                        autoContainerWidth
                        autoContainerHeight
                        autoHeight
                        scrollTop={this.state.scrollTop}
                      />
                    </div>)}
                </ColumnSizer>)
            }}
          </AutoSizer>
        </div>
      </div>
    )
  }
}

export default injectSheet(styles)(GameGridView)