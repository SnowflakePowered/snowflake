import React from 'react';
import injectSheet from 'react-jss' 
import styleable from '../../../utils/styleable'
import InfoDisplay from './InfoDisplay'
const styles = {
  
}

const GameDisplay = ({gameTitle, year, publisher, region}) => (
  <InfoDisplay title={gameTitle} subtitle={publisher} metadata={[year, region]}/>
)


export default injectSheet(styles)(styleable(GameDisplay))
