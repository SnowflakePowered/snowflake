import * as React from 'react'
import injectSheet, { StyleProps } from "support/InjectSheet";

import Paper from 'material-ui/Paper'
import FormControl from 'material-ui/Form/FormControl'
import Input from 'material-ui/Input/Input'
import SearchIcon from 'material-ui-icons/Search'

import grey from 'material-ui/colors/grey'

import { styles } from './SearchBar.style'

type SearchBarProps = {
  tagline?: string,
  onChange?: (e: string) => void
}

const SearchBar: React.SFC<SearchBarProps & StyleProps> = ({ classes, tagline, onChange }) => (
  <Paper className={classes.barContainer}>
    <SearchIcon color={grey[400]} className={classes.searchIcon} />
    <FormControl className={classes.input}>
      <Input
          placeholder={'Search ' + (tagline || '')}
          className={classes.textFieldUnderline}
          onChange={onChange}
      />
    </FormControl>
  </Paper>
)

export default injectSheet(styles)(SearchBar)
