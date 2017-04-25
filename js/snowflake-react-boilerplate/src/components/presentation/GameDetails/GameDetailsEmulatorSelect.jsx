import React from 'react'
import DropDownMenu from 'material-ui/DropDownMenu'
import MenuItem from 'material-ui/MenuItem'

const GameDetailsEmulatorSelect = ({emulators, onChange}) => (
   <DropDownMenu value={1} onChange={onChange}>
          <MenuItem value={1} primaryText="Nestopia (Retroarch)" />
          <MenuItem value={2} primaryText="Mesen" />
    </DropDownMenu>
)

export default GameDetailsEmulatorSelect