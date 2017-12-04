import * as React from 'react'
import * as PropTypes from 'prop-types'

import InfoDisplay from 'components/InfoDisplay/InfoDisplay'

type GameDisplayProps = {
  title: string,
  year?: string,
  publisher?: string,
  region?: string
}

const GameDisplay: React.SFC<GameDisplayProps> = ({title, year, publisher, region}) => (
  <InfoDisplay title={title} subtitle={publisher} metadata={[year!, region!]}/>
)

GameDisplay.propTypes = {
  title: PropTypes.string.isRequired,
  year: PropTypes.number,
  publisher: PropTypes.string,
  region: PropTypes.string
}
export default GameDisplay
