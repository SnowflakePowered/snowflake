import React from 'react'
import { Link as ReactRouterLink} from 'react-router-dom'

const Link = (props) => {
  return (
    <ReactRouterLink {...props} style={{ textDecoration: 'none', backgroundColor: 'inherit', color: 'inherit' }} />
  )
}

export default Link
