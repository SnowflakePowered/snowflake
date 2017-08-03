import * as React from 'react'
import * as PropTypes from 'prop-types'
import injectSheet, { StyleProps } from 'support/InjectSheet'

import { styles } from './InfoDisplay.style'

type InfoDisplayProps = {
  title?: string,
  subtitle?: string,
  tagline?: string,
  metadata?: string[],
  stats?: string[]
}

const InfoDisplay: React.SFC<InfoDisplayProps & StyleProps> = ({ classes, title, subtitle, tagline, metadata, stats }) => (
  <div className={classes.container}>
    <div className={classes.top}>
      <div className={classes.title}>{title || ''}</div>
      <div className={classes.subTitle}>{subtitle || ''}</div>
      <div className={classes.tagline}>{tagline || ''}</div>
      {(metadata || []).map(m => <div className={classes.metadata} key={m}>{m || ''}</div>)}
    </div>
    <div className={classes.bottom}>
      {(stats || []).map(s => <div className={classes.stats} key={s}>{s || ''}</div>)}
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
