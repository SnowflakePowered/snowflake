import React from 'react'
import { storiesOf, action, linkTo } from '@kadira/storybook'
import centered from '@kadira/react-storybook-decorator-centered'
import full from './utils/full'
import GameCardStory, {Single} from './GameCardStory.story'
import mui from './utils/mui'
import { blue, pink } from 'material-ui/styles/colors'

import {SearchBarStory, SearchBarStoryWithBackground} from './SearchBarStory.story'
import {Black, WhiteWithBackground} from './PlatformDisplay.story'
import {GameBlack, GameWhiteWithBackground} from './GameDisplay.story'
//import { GameDetailsStory } from './GameDetails.story'
import { BooleanConfigurationStory } from './Configuration.story'

const muiTheme = mui({blue, pink})

storiesOf('Game Card', module)
  .addDecorator(centered)
  .addDecorator(muiTheme)
  .add('Multiple', () => <GameCardStory/>)
  .add('Single', () => <Single/>)

storiesOf('Platform Display', module)
  .addDecorator(full)
  .addDecorator(muiTheme)
  .add('on White', () => <Black/>)
  .add('on Background', () => <WhiteWithBackground/>)  

storiesOf('Game Display', module)
  .addDecorator(full)
  .addDecorator(muiTheme)
  .add('on White', () => <GameBlack/>)
  .add('on Background', () => <GameWhiteWithBackground/>)

storiesOf('Search Bar', module)
  .addDecorator(centered)
  .addDecorator(muiTheme)
  .add('Search bar', () => <SearchBarStory/>)
  .add('with Background', () => <SearchBarStoryWithBackground/>)

storiesOf('Configuration', module)
  .addDecorator(centered)
  .addDecorator(muiTheme)
  .add('boolean widget', () => <BooleanConfigurationStory/>)

/*
storiesOf('Game Details', module)
  .addDecorator(centered)
  .add('on White', () => <GameDetailsStory/>)

 */