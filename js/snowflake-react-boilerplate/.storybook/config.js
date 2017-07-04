import { configure } from '@storybook/react';
import './styles.css'
import 'babel-polyfill'
import Perf from 'react-addons-perf'
window.Perf = Perf

function loadStories() {
  require('../src/stories');
}

configure(loadStories, module);
