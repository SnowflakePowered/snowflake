import * as React from 'react'
import * as PropTypes from 'prop-types'

import Collapse from 'material-ui/transitions/Collapse'
import Typography from 'material-ui/Typography'

import injectSheet, { StyleProps } from 'support/InjectSheet'
import * as classnames from 'classnames'
import ExpandMoreIcon from 'material-ui-icons/ExpandMore'
import IconButton from 'material-ui/IconButton'

import { styles } from './CollapsingParagraph.style'

type CollapsingParagaphProps = {
  children: string
}

type CollapsingParagaphState = {
  open: boolean,
  isCollapsing: boolean
}

class CollapsingParagaph extends React.Component<CollapsingParagaphProps & StyleProps, CollapsingParagaphState> {
  static propTypes = {
    children: PropTypes.string
  }

  state: CollapsingParagaphState = {
    open: false,
    isCollapsing: false
  }

  handleExpandClick = () => {
    this.setState({ open: !this.state.open, isCollapsing: true })
  }

  handleCollapsing = () => {
    this.setState({...this.state, isCollapsing: false})
  }

  render () {
    const classes = this.props.classes
    const text = this.props.children
    // todo: calculate text size.
    return (
      <div className={classes.container}>
        <div className={classes.textSection}>
          <Collapse in={this.state.open}
            transitionDuration='auto'
            onExiting={this.handleCollapsing}
            className={ !this.state.isCollapsing ? classes.collapse : ''}>
            <Typography paragraph={true} className={classes.paragraph}>
                { text }
            </Typography>
          </Collapse>
        </div>
        <div className={classes.toolbar}>
          <IconButton
                className={
                  classnames(classes.expand, {
                    [classes.expandOpen]: this.state.open
                  })}
                onClick={this.handleExpandClick}
                aria-expanded={this.state.open}
                aria-label='Show more'
              >
                <ExpandMoreIcon />
          </IconButton>
        </div>
      </div>
    )
  }
}

export default injectSheet(styles)(CollapsingParagaph)
