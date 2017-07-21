import * as React from 'react'
import Switch from 'material-ui/Switch'

import ConfigurationWidget from 'components/ConfigurationWidget/ConfigurationWidget'

const BooleanWidget = ({classes, name, description, value, onValueChange}) => (
  <ConfigurationWidget name={name} description={description}>
    <Switch/>
  </ConfigurationWidget>
)

export default BooleanWidget
