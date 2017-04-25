import { configure } from '@kadira/storybook';
import './styles.css'

function loadStories() {
  require('../src/stories');
}

configure(loadStories, module);
