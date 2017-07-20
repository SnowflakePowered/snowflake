import * as React from 'react'
import { Route } from 'react-router'
import ActivePlatformRouter from 'routing/ActivePlatformRouter'
import ActiveGameRouter from 'routing/ActiveGameRouter'

const MapRouteToState = () => (
  <div>
    <Route path='*' component={ActiveGameRouter}/>
    <Route path='*' component={ActivePlatformRouter}/>
  </div>
)

export default MapRouteToState
