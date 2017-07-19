import React from 'react'
import PropTypes from 'prop-types'

import Collapse from 'material-ui/transitions/Collapse'
import Typography from 'material-ui/Typography'

import injectSheet from 'mui-jss-inject'
import classnames from 'classnames'
import ExpandMoreIcon from 'material-ui-icons/ExpandMore'
import IconButton from 'material-ui/IconButton'

const LINE_HEIGHT = 1.5

const styles = {
  expand: {
    transform: 'rotate(0deg)'
  },
  expandOpen: {
    transform: 'rotate(180deg)',
  },
  container: {
    display: 'grid',
    gridTemplateColumns: '[textSection] auto [toolbar] min-content'
  },
  textSection: {
    gridColumn: 'textSection'
  },
  toolbar: {
   
    gridColumn: 'toolbar'
  },
  collapse: {
    // Override the MUI Collapse to show the first line of the text.
    // 1em * 1.5 value is given by (
    // (relative line-height) = (absolute line-height) / (font-size) =>
    // (absolute line-height) = (relative line-height) * (font-size).
    height: `calc(1em * ${LINE_HEIGHT}) !important`
  },
  paragraph: {
    lineHeight: LINE_HEIGHT
  }
}

const TRUNCATE_LENGTH = 100

class CollapsingParagaph extends React.Component { 
  state = {
    open: false,
    isCollapsing: false
  }
  

  handleExpandClick = () => {
    this.setState({ open: !this.state.open, isCollapsing: true });
  }



  handleCollapsing = () => {
    this.setState({...this.state, isCollapsing: false})
  }

  render() {
    const classes = this.props.classes
    const text = this.props.children
    // todo: calculate text size.
    return (
      <div className={classes.container}>
        <div className={classes.textSection}>
          <Collapse in={this.state.open} 
            transitionDuration="auto" 
            onExited={this.handleFinishedCollapsing}
            onExiting={this.handleCollapsing}
            className={ !this.state.isCollapsing ? classes.collapse : ""}>
            <Typography paragraph className={classes.paragraph}>
                { text }
            </Typography>
          </Collapse>
        </div>
        <div className={classes.toolbar}>
          <IconButton
                className={
                  classnames(classes.expand, {
                    [classes.expandOpen]: this.state.open,
                  })}
                onClick={this.handleExpandClick}
                aria-expanded={this.state.open}
                aria-label="Show more"
              >
                <ExpandMoreIcon />
          </IconButton>
        </div>
      </div>
    )
  }
}

CollapsingParagaph.propTypes = {
  children: PropTypes.string
}

export default injectSheet(styles)(CollapsingParagaph)
