import React from 'react'
import Switch from 'material-ui/Switch';

import ConfigurationWidgetTemplate from './ConfigurationWidgetTemplate'

const BooleanWidget = ({classes, name, description, value, onValueChange}) => (
  <ConfigurationWidgetTemplate name={name} description={description}>
    <Switch/>
  </ConfigurationWidgetTemplate>
)

export default BooleanWidget