import React from 'react'
import Toggle from 'material-ui/Toggle';

import ConfigurationWidgetTemplate from './ConfigurationWidgetTemplate'
const BooleanWidget = ({name, description, value, onValueChange}) => (
  <ConfigurationWidgetTemplate name={name} description={description}>
    <Toggle toggled={value}/>
  </ConfigurationWidgetTemplate>
)

export default BooleanWidget