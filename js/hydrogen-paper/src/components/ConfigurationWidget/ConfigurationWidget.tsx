import * as React from 'react'
import injectSheet, { StyleProps } from 'support/InjectSheet'

const sheet = {
  container: {
    fontFamily: 'Roboto, sans-serif',
    display: 'grid',
    width: '100%',
    gridTemplateColumns: '80% 20%'
  },
  description: {

  },
  control: {
    justifySelf: 'end'
  },
  configTitle: {

  },
  configDescription: {
    color: 'grey',
    fontSize: '0.75em'
  }
}

type ConfigurationWidgetProps = {
  name: string,
  description: string
}
const ConfigurationWidget: React.SFC<ConfigurationWidgetProps & StyleProps> = ({classes, name, description, children}) => (
  <div className={classes.container}>
    <div className={classes.description}>
      <div className={classes.configTitle}>
        {name}
      </div>
      <div className={classes.configDescription}>
        {description}
      </div>
    </div>
    <div className={classes.control}>
      {children}
    </div>
  </div>
)

export default injectSheet<ConfigurationWidgetProps>(sheet)(ConfigurationWidget)
