import React from 'react';
import { storiesOf, action, linkTo } from '@kadira/storybook';
import centered from '@kadira/react-storybook-decorator-centered';
import {muiTheme} from 'storybook-addon-material-ui';
import GameCardStory, {Single} from './GameCardStory.story'

import SearchBar from '../components/presentation/SearchBar'

storiesOf('Game Card', module)
  .addDecorator(centered)
  .addDecorator(muiTheme())
  .add('Multiple', () => <GameCardStory/>)
  .add('Single', () => <Single/>);


storiesOf('Search Bar', module)
  .addDecorator(centered)
  .addDecorator(muiTheme())
  .add('Search bar', () => <SearchBar tagline="Nintendo Entertainment System" onChange={action('text-changed')}/>);