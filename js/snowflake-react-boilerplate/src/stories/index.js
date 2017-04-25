import React from 'react';
import { storiesOf, action, linkTo } from '@kadira/storybook';
import centered from '@kadira/react-storybook-decorator-centered';
import full from './full'
import {muiTheme} from 'storybook-addon-material-ui';
import GameCardStory, {Single} from './GameCardStory.story'
import {SearchBarStory, SearchBarStoryWithBackground} from './SearchBarStory.story'
import {Black, WhiteWithBackground} from './PlatformDisplay.story'
import {GameBlack, GameWhiteWithBackground} from './GameDisplay.story'
import { GameDetailsStory } from './GameDetails.story'
import { BooleanConfigurationStory } from './Configuration.story'

storiesOf('Game Card', module)
  .addDecorator(centered)
  .addDecorator(muiTheme())
  .add('Multiple', () => <GameCardStory/>)
  .add('Single', () => <Single/>)

storiesOf('Search Bar', module)
  .addDecorator(centered)
  .addDecorator(muiTheme())
  .add('Search bar', () => <SearchBarStory/>)
  .add('with Background', () => <SearchBarStoryWithBackground/>);

storiesOf('Platform Display', module)
  .addDecorator(full)
  .addDecorator(muiTheme())
  .add('on White', () => <Black/>)
  .add('on Background', () => <WhiteWithBackground/>)  

storiesOf('Game Display', module)
  .addDecorator(full)
  .addDecorator(muiTheme())
  .add('on White', () => <GameBlack/>)
  .add('on Background', () => <GameWhiteWithBackground/>)

  
storiesOf('Game Details', module)
  .addDecorator(centered)
  .addDecorator(muiTheme())
  .add('on White', () => <GameDetailsStory/>)

storiesOf('Configuration', module)
  .addDecorator(centered)
  .addDecorator(muiTheme())
  .add('boolean widget', () => <BooleanConfigurationStory/>)