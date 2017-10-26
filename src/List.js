import React, { Component } from 'react';

import SlotModify from './Modify.js';
import SlotView from './View.js';

import $ from './jquery-ajax-fake.js';

class ListSlots extends Component {
  constructor(props) {
    super(props);

    this.state = {selected: "none", data: []};

    this.expandSlot = this.expandSlot.bind(this);
    this.createSlot = this.createSlot.bind(this);
    this.saveSlot = this.saveSlot.bind(this);
  }

  componentDidMount() {
    $.ajax({
      url: this.props.url,
      success: function(data) {
        this.setState({data: JSON.parse(data)});
      }.bind(this)
    });
  }

  expandSlot(event) {
    this.setState({selected: parseInt(event.currentTarget.getAttribute('data-key'), 10)});
  }

  createSlot(event) {
    this.setState({selected: "new"});    
  }

  saveSlot(event) {
    var data = event.currentTarget;
    console.log(data);
    $.ajax({
      url: this.props.url,
      type: 'POST',
      data: "start=2018-08-03T10%3A00%3A00&end=2018-08-03T19%3A00%3A00&description=hello&project=Test",
      success: function(data) {
        this.setState({data: JSON.parse(data)});
      }.bind(this)
    });

  }

  render() {
    console.log(this.state);
    var component = this;
    var children = this.state.data.map(function(item){
      if (component.state && component.state.selected === parseInt(item.ID, 10)) {
        return (
<div className="list-group-item" key={item.ID}>
  <SlotModify data={item} />        
  <button onClick={component.saveSlot} data-key={item.ID} className="btn btn-primary btn-block">Save</button>       
</div>
        )
      } else {
        return (
<div className="list-group-item" key={item.ID}>
  <SlotView data={item} />        
  <button onClick={component.expandSlot} data-key={item.ID} className="btn btn-primary btn-block">Expand</button>
</div>
        );
      }
    });
    if (this.state.selected === "new") {
      var item = {ID: 0,Start: "2018-08-03T09:00:00",End: "2018-08-03T18:00:00",Description: "New work slot",Project: "Unspecified"};
      console.log(item);
      return (
<div>
  <ul className="list-group">      
    <div className="list-group-item" key={item.ID}>
      <SlotModify data={item} />        
      <button onClick={this.saveSlot} data-key={item.ID} className="btn btn-primary btn-block">Save</button>       
    </div>
    {children}
  </ul>
</div>   
      )
    } else {
      return (
<div>
  <button onClick={this.createSlot} className="btn btn-primary btn-block">Add Time Slot</button>       
  <ul className="list-group">      
    {children}
  </ul>
</div>
      )   
    }
  }
}

export default ListSlots;
