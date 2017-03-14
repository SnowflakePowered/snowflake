import React, { Component } from 'react';
import logo from './logo.svg';
import './App.css';
import Test from './components/test';

class App extends Component {
  componentDidMount() {

  }
  
  render() {
    return (
      <div className="App">
        <div className="App-header">
          <img src={logo} className="App-logo" alt="logo" />
          <h2>Welcome to React</h2>
        </div>
        <div className="App-intro">
          To get started, edit <code>src/App.js</code> and save to reload.
          <Test/>
        </div>
      </div>
    );
  }
}

export default App;
