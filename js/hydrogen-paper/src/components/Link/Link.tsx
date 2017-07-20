import * as React from 'react'
import { Link as ReactRouterLink, LinkProps } from 'react-router-dom'

const Link: React.SFC<LinkProps> = (props) => {
  return (
    <ReactRouterLink {...props} style={{
      textDecoration: 'none',
      backgroundColor: 'inherit',
      color: 'inherit' }} />
  )
}

export default Link
