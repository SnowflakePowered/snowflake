import * as React from 'react'
import injectSheet, { StyleProps } from 'support/InjectSheet'
import { AutoSizer, Grid, ColumnSizer, WindowScroller } from 'react-virtualized'
import { dimensions } from 'components/GameCard/GameCard.style'
import { styles } from './GameCardGrid.style'

const cellRenderer = ({ className, children, numberOfRows, numberOfColumns }: {className: string, children: React.ReactChild[], numberOfRows: number, numberOfColumns: number}) =>
 ({ columnIndex, key, rowIndex, style }) => {
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
  let dimensionObject
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

type GameCardGridProps = {
  portrait?: boolean,
  landscape?: boolean,
  square?: boolean,
  header?: React.ReactNode
}

type GameCardGridState = {
  scrollElement?: HTMLElement | null
}

class GameCardGrid extends React.PureComponent<GameCardGridProps & StyleProps, GameCardGridState> {

  state = {
    scrollElement: null
  }

  constructor (props) {
    super(props)
  }

  updateScrollElement (target, containerClass) {
    if (!this.state.scrollElement && target.classList.contains(containerClass)) {
      this.setState({ scrollElement: target })
    }
  }

  handleScroll = (e) => this.updateScrollElement(e.target, this.props.classes.container)

  render () {
    const { BOX_HEIGHT, BOX_WIDTH } = getDimensions(this.props.portrait, this.props.landscape, this.props.square)
    const children = React.Children.toArray(this.props.children)
    return (
      <div className={this.props.classes.container}
        onScroll={this.handleScroll} // hackity hack
      >
        <div className={this.props.classes.autoSizerContainer}>
          <WindowScroller
            scrollElement={this.state.scrollElement!}
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
                    const cells = cellRenderer({
                      className: this.props.classes.cellWrapper,
                      children: children,
                      numberOfRows,
                      numberOfColumns
                    })
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

export default injectSheet(styles)(GameCardGrid)
