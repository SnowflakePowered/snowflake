import React from 'react'
import injectSheet from 'react-jss' 

import Paper from 'material-ui/Paper'
import TextField from 'material-ui/TextField';
import SearchIcon from 'material-ui/svg-icons/action/search'

import styleable from '../../utils/styleable'

const styles = {
  barContainer: {
    width: '90%',
    display: 'grid',
    gridTemplateColumns: '32px auto 10px',
    alignItems: 'center'
  },
  searchIcon: {
    justifySelf: 'center'
  }
}

const SearchBar = ({classes, tagline, onChange}) => (
  <Paper className={classes.barContainer}>
    <SearchIcon className={classes.searchIcon}/><TextField onChange={onChange} fullWidth={true} hintText={"Search " + (tagline || "")}/>
  </Paper>
)

export default injectSheet(styles)(SearchBar);