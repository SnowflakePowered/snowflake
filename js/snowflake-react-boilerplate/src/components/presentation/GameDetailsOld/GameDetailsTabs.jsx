import React from 'react'
import injectSheet from 'react-jss'
import {Tabs, Tab} from 'material-ui/Tabs'

const sheet = {
  tabBackgroundStyle: {
    backgroundColor: "#fff"
  },
  tabTextStyle: {
    color: "#000"
  }
}
const GameCardTabs = ({classes, sheet}) => (
  <Tabs tabItemContainerStyle={sheet.rules.raw.tabBackgroundStyle} inkBarStyle={sheet.rules.raw.tabTextStyle}>
    <Tab label="Settings" style={sheet.rules.raw.tabTextStyle}>
      <div>
       
      </div>
    </Tab>
    <Tab label="Saves" style={sheet.rules.raw.tabTextStyle}>
      <div>
        Coming Soon!
      </div>
    </Tab>
    <Tab label="Advanced" style={sheet.rules.raw.tabTextStyle}>
      <div>
        
      </div>
    </Tab>
</Tabs>
)

export default injectSheet(sheet)(GameCardTabs)