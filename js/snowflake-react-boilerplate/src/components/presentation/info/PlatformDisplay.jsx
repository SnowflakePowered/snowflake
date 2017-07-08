import React from 'react'
import injectSheet from 'react-jss'
import InfoDisplay from './InfoDisplay'

const styles = {
}

const PlatformDisplay = ({platformName, platformAltName, publisher, year, gameCount, emulatorNames}) => (
  <InfoDisplay subtitle={platformName} tagline={platformAltName} metadata={[publisher, year]} stats={[(gameCount || 0) + ' games', emulatorNames]}/>
)

export default injectSheet(styles)(PlatformDisplay)
