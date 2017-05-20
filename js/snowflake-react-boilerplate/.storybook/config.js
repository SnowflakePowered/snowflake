import { configure } from '@storybook/react';
import './styles.css'

function loadStories() {
  require('../src/stories');
}

configure(loadStories, module);
