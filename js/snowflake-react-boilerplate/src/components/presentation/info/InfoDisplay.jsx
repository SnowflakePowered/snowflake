import React from 'react'
import PropTypes from 'prop-types'

import injectSheet from 'react-jss'

const styles = {
  '@font-face': {
    fontFamily: 'Roboto'
  },
  container: {
    fontFamily: 'Roboto, Noto Sans, sans-serif',
    display: 'grid',
    gridTemplateRows: '50% 50%'
  },
  title: {
    fontSize: '1.5em',
    wordBreak: 'break-all'
  },
  subTitle: {
    fontSize: '1em'
  },
  tagline: {
    fontSize: '0.9em'
  },
  metadata: {
    fontSize: '0.9em',
    fontWeight: 200,
    fontStyle: 'oblique'
  },
  top: {
    gridRow: 1,
    alignSelf: 'start'
  },
  bottom: {
    gridRow: 2,
    alignSelf: 'end'
  }
}

const InfoDisplay = ({ classes, title, subtitle, tagline, metadata, stats }) => (
  <div className={classes.container}>
    <div className={classes.top}>
      <div className={classes.title}>{title || ''}</div>
      <div className={classes.subTitle}>{subtitle || ''}</div>
      <div className={classes.tagline}>{tagline || ''}</div>
      {(metadata || []).map(m => <div className={classes.metadata} key={m}>{m || ''}</div>)}
    </div>
    <div className={classes.bottom}>
      {(stats || []).map(s => <div className={classes.stats}>{s || ''}</div>)}
    </div>
  </div>
)

InfoDisplay.propTypes = {
  title: PropTypes.string,
  subtitle: PropTypes.string,
  tagline: PropTypes.string,
  metadata: PropTypes.arrayOf(PropTypes.oneOfType([
    PropTypes.string,
    PropTypes.number
  ])),
  stats: PropTypes.arrayOf(PropTypes.oneOfType([
    PropTypes.string,
    PropTypes.number
  ]))
}

export default injectSheet(styles)(InfoDisplay)
