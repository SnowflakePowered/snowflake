import React from 'react';
import { storiesOf, action, linkTo } from '@kadira/storybook';
import centered from '@kadira/react-storybook-decorator-centered';
import {muiTheme} from 'storybook-addon-material-ui';
import GameCardStory from './GameCardStory.story'

storiesOf('Game Card', module)
  .addDecorator(centered)
  .addDecorator(muiTheme())
  .add('With Image', () => <GameCardStory/>);