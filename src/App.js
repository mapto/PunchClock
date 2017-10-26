import React, { Component } from 'react';

import './main.css';

import ListSlots from './List.js';

class App extends Component {
  render() {
    return (
<div>
  <header className="page-header">
    <h1 className="title">PunchClock <small>Time Registration</small></h1>
  </header>
  <div className="container">
  <div className="row">
    <div className="panel panel-default">
      <div className="panel-body">
        <ListSlots url="http://localhost:8080/api/Time" />
      </div>
    </div>
  </div>
  </div>
</div>
    );
  }
}

export default App;
