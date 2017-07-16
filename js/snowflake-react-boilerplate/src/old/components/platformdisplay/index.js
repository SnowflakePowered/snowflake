import React from 'react'

const PlatformDisplay = ({platform}) => {
  return (
    <PlatformDisplayInfo platform={platform}/>
  )
}

const PlatformDisplayInfo = ({platform}) => {
  return (
    <div className="platformInfo">
      <div>{platform.FriendlyName || ''}</div>
      <div>{platform.Metadata.platform_company || ''}</div>
      <div>{platform.Metadata.platform_releasedate || ''}</div>
    </div>
  )
}

export default PlatformDisplay
