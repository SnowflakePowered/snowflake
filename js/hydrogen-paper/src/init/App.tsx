import * as React from 'react'
import SnowflakeProvider from 'state/SnowflakeProvider'
// import GameCard from 'components/GameCard/GameCard'
import Switchboard from './Switchboard'

class App extends React.Component<{}, {}> {
  render () {
    return (
      <SnowflakeProvider>
        <div>
          <Switchboard/>
        </div>
      </SnowflakeProvider>
    )
  }
}

export default App
