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

import ImageCard from 'components/presentation/ImageCard'
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

storiesOf('Image Card', module)
  .addDecorator(centered)
  .addDecorator(muiTheme)
  .add('portrait', () => 
  <ImageCard
    image="http://vignette2.wikia.nocookie.net/mario/images/6/60/SMBBoxart.png/revision/latest?cb=20120609143443"
  />)
  .add('landscape', () => 
  <ImageCard
    image="https://upload.wikimedia.org/wikipedia/en/3/32/Super_Mario_World_Coverart.png"
  />)

/*
storiesOf('Game Details', module)
  .addDecorator(centered)
  .add('on White', () => <GameDetailsStory/>)

 */