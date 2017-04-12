import React from 'react'
import PlatformDisplay from '../platformdisplay'
import { List, ListItem } from 'material-ui/List'
import { Link } from 'react-router-dom'
import './index.css'

import withPlatforms from '../../snowflake/withPlatforms'

const PlatformSelector = ({ platforms }) => {
  return (
    <List className="list">
      {
        [...platforms.values()].map((p) => {
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

export default withPlatforms(PlatformSelector)
