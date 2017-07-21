import * as React from 'react'
import injectSheet from 'mui-jss-inject'

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
  classes?: any,
  name: string,
  description: string
}
const ConfigurationWidget: React.SFC<ConfigurationWidgetProps> = ({classes, name, description, children}) => (
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

export default injectSheet(sheet)(ConfigurationWidget)
