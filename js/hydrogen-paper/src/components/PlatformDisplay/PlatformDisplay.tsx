import * as React from 'react'
import InfoDisplay from 'components/InfoDisplay/InfoDisplay'

type PlatformDisplayProps = {
  platformName: string,
  platformAltName?: string,
  publisher?: string,
  year?: string,
  gameCount?: number
}

const PlatformDisplay: React.SFC<PlatformDisplayProps> = ({platformName, platformAltName, publisher, year, gameCount}) => (
  <InfoDisplay subtitle={platformName}
               tagline={platformAltName}
               metadata={[year!, publisher!]}
               stats={[(gameCount || 0).toString() + ' games']}
              />
)

export default PlatformDisplay
