import React from 'react'
import PlatformDisplay from './platformdisplay'
import { List, ListItem } from 'material-ui/List'
import { Link } from 'react-router-dom'
import './platformselector.css'

const PlatformSelector = ({ snowflake }) => {
  return (
    <List className="list">
      {
        [...snowflake.stone.platforms.values()].map(p => {
          return (
            <Link key={p.PlatformID} to={{
              pathname: `/games/${p.PlatformID}`,
              state: {
                platform: p
              }
            }}>
              <ListItem>
                <PlatformDisplay platform={p} />
              </ListItem>
            </Link>
          )
        })
      }
    </List>
  )
}

export default PlatformSelector
