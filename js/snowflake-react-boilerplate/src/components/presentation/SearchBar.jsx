import React from 'react'
import injectSheet from 'react-jss' 

import Paper from 'material-ui/Paper'
import TextField from 'material-ui/TextField';
import SearchIcon from 'material-ui/svg-icons/action/search'

import { grey400, red400 } from 'material-ui/styles/colors'

import styleable from 'utils/styleable'

const styles = {
  barContainer: {
    width: '100%',
    display: 'grid',
    gridTemplateColumns: '48px auto 10px',
    alignItems: 'center',
    opacity: 0.4,
    '&:hover, &:focus': {
      opacity: 1
    }
  },
  searchIcon: {
    justifySelf: 'center'
  },
  textFieldUnderline: {
    borderColor: red400
  }
}

const SearchBar = ({classes, sheet, tagline, onChange}) => (
  <Paper className={classes.barContainer}>
    <SearchIcon color={grey400} className={classes.searchIcon}/>
    <TextField underlineFocusStyle={sheet.rules.raw.textFieldUnderline} onChange={onChange} fullWidth={true} hintText={"Search " + (tagline || "")}/>
  </Paper>
)

export default injectSheet(styles)(SearchBar);