import React from 'react'
import PropTypes from 'prop-types'
import injectSheet from 'mui-jss-inject'
import { AutoSizer, Grid, ColumnSizer, WindowScroller } from 'react-virtualized'
import GameCard, { dimensions } from './GameCard'
import GameCardAdapter from 'components/adapter/GameCardAdapter'

const styles = {
  container: {
    display: 'flex',
    width: '100%',
    height: '100%',
    flexDirection: 'column',
    overflowY: 'auto',
    overflowX: 'hidden',
    '-webkit-select': 'none'
  },
  card: {
    padding: 10,
    display: 'inline-block'
  },
  autoSizerContainer: {
    height: '100%',
  },
  cellWrapper: {
    display: 'flex',
    justifyContent: 'center',
    alignItems: 'center'
  },
  gridContainer: {
    display: 'flex',
    justifyContent: 'center',
    margin: [10, 10]
  }
}

const cellRenderer = ({ className, children, numberOfRows, numberOfColumns }) => ({ columnIndex, key, rowIndex, style }) => {
  return (
    <div key={key}
      style={style}
      className={className}
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

class GameGrid extends React.PureComponent {

  constructor(props) {
    super(props)
    this.state = {
      scrollElement: null
    }
  }

  updateScrollElement(target, containerClass) {
    if (this.state.scrollElement === null && target.classList.contains(containerClass)) {
      this.setState({ scrollElement: target })
    }
  }
 
  handleScroll = (e) => this.updateScrollElement(e.target, this.props.classes.container)

  render() {
    const { BOX_HEIGHT, BOX_WIDTH } = getDimensions(this.props.portrait, this.props.landscape, this.props.square)
    const children = React.Children.toArray(this.props.children)
    return (
      <div className={this.props.classes.container}
        onScroll={this.handleScroll} //hackity hack
      >
        <div className={this.props.classes.autoSizerContainer}>
          <WindowScroller
            scrollElement={this.state.scrollElement}
          >
            {({ height, scrollTop, isScrolling }) => (
              <div>
                {
                  this.props.header
                }
                <AutoSizer disableHeight>
                  {({ width }) => {
                    const numberOfColumns = Math.floor(width / BOX_WIDTH)
                    const CENTERED_BOX_WIDTH = BOX_WIDTH + (BOX_WIDTH / numberOfColumns / 2)
                    const numberOfRows = Math.ceil(children.length / numberOfColumns)
                    const cells = cellRenderer({ classes: this.props.classes.cellWrapper,
                                                 children: children,
                                                 numberOfRows, numberOfColumns })
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
                              cellRenderer={cells}
                              columnCount={numberOfColumns}
                              rowCount={numberOfRows}
                              columnWidth={getColumnWidth}
                              rowHeight={BOX_HEIGHT}
                              height={height}
                              isScrolling={isScrolling}
                              width={adjustedWidth}
                              autoContainerWidth
                              autoHeight
                              overscanRowCount={2}
                              scrollTop={scrollTop}
                            />
                          </div>)}
                      </ColumnSizer>)
                  }}
                </AutoSizer>
              </div>)}
          </WindowScroller>
        </div>
      </div>
    )
  }
}

export default injectSheet(styles)(GameGrid)