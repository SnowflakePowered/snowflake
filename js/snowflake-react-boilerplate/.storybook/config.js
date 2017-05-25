import { configure } from '@storybook/react';
import './styles.css'

import Perf from 'react-addons-perf'
window.Perf = Perf

function loadStories() {
  require('../src/stories');
}

configure(loadStories, module);
