import React from 'react'
import injectSheet from 'react-jss'
import PropTypes from 'prop-types'

import InfoDisplay from './InfoDisplay'

const styles = {
}

const GameDisplay = ({title, year, publisher, region}) => (
  <InfoDisplay title={title} subtitle={publisher} metadata={[year, region]}/>
)

GameDisplay.propTypes = {
  title: PropTypes.string.isRequired,
  year: PropTypes.number,
  publisher: PropTypes.string,
  region: PropTypes.string
}
export default injectSheet(styles)(GameDisplay)
