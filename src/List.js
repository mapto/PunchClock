import React, { Component } from 'react';

import SlotModify from './Modify.js';
import SlotView from './View.js';

import $ from './jquery-ajax-fake.js';

class ListSlots extends Component {
  constructor(props) {
    super(props);

    var now = new Date();
    this.state = {
      selected: "none",
      projects: ["All"],
      data: [],
      filter: ["All", now.getUTCFullYear(), now.getUTCMonth()+1]
    };

    this.updateFilter = this.updateFilter.bind(this);

    this.expandSlot = this.expandSlot.bind(this);
    this.createSlot = this.createSlot.bind(this);
    this.saveSlot = this.saveSlot.bind(this);

    this.updateFilter();
  }

  reload() {
    console.log(this.state.filter);
    $.ajax({
      url: this.props.url + '/Filter/' + this.state.filter.join("/"),
      success: function(data) {
        this.setState({selected: "none", data: JSON.parse(data)});
      }.bind(this)
    });

  }

  componentWillMount() {
    $.ajax({
      url: this.props.url + '/Project',
      success: function(data) {
        this.setState({projects: ["All"].concat(JSON.parse(data))});
      }.bind(this)
    })
    this.reload();
  }

  componentDidMount() {
  }

  updateFilter(event) {
      var mapping = {
        projectFilter: 0,
        yearFilter: 1,
        monthFilter: 2,
        dayFilter: 3
      }

      if (event) {
        var filter = this.state.filter;
        filter[mapping[event.target.id]] = event.target.value;
        this.setState(filter: filter);        
      }
      this.reload();
  }

  expandSlot(event) {
    this.setState({selected: parseInt(event.currentTarget.getAttribute('data-key'), 10)});
  }

  createSlot(event) {
    this.setState({selected: "new"});    
  }

  saveSlot(event) {
    var data = [];
    // TODO: Wanted to have this via JSON for consistency, but didn't have time to experiment how WebApi delivers to back-end Controller method
    for (var el of document.getElementsByClassName("slot-form")[0].getElementsByTagName("input")) {
      data.push(el.name + "=" + el.value);
    }
    data = data.join("&");
    $.ajax({
      url: this.props.url + '/Time',
      type: 'POST',
      data: data,
      contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
      success: function(data) {
        // TODO: Instead of reloading everything from server, better update item locally.
        this.reload();
      }.bind(this)
    });

  }

  render() {
    var filter = (
<div className="filter-form form-inline">
  <div className="col-sm-offset-2 col-sm-1">
    <label htmlFor="filter" className="control-label">Filter</label>
    <input type="hidden" id="filter" className="form-control"/>
  </div>
  <div className="col-sm-2">
    <label htmlFor="projectFilter" className="control-label">Project</label>
    <select id="projectFilter" value={this.state.filter[0]} onChange={this.updateFilter} className="form-control">
        {
          this.state.projects.map(function(item){
            return(
              <option value={item} key={item}>{item}</option>
            )
          })            
          }
        }
    </select>
  </div>
  <div className="col-sm-2">
    <label htmlFor="yearFilter" className="control-label">Year</label>
    <input type="number" defaultValue={new Date().getUTCFullYear()} id="yearFilter" onChange={this.updateFilter} className="form-control" min="2000" max="2030"/>
  </div>
  <div className="col-sm-1">
    <label htmlFor="monthFilter" className="control-label">Month</label>
    <input type="number" defaultValue={new Date().getUTCMonth()+1} id="monthFilter" onChange={this.updateFilter} className="form-control" min="1" max="12"/>
  </div>
  <div className="col-sm-1">
    <label htmlFor="dayFilter" className="control-label">Day</label>
    <input type="number" id="dayFilter" onChange={this.updateFilter} className="form-control" min="1" max="31"/>
  </div>
</div>
      )
    var component = this;
    var children;
    if (this.state.data.length) {
      children = this.state.data.map(function(item){
        if (component.state && component.state.selected === parseInt(item.ID, 10)) {
          return (
  <li className="list-group-item row" key={item.ID}>
    <SlotModify data={item} />        
    <button onClick={component.saveSlot} data-key={item.ID} className="btn btn-primary btn-block">Save</button>       
  </li>
          )
        } else {
          return (
  <li className="list-group-item row" key={item.ID}>
    <SlotView data={item} />        
    <button onClick={component.expandSlot} data-key={item.ID} className="btn btn-primary btn-block">Expand</button>
  </li>
          );
        }
      });
    } else {
      children = (
<li className="list-group-item">No timeslots.</li>
      );
    }

    if (this.state.selected === "new") {
      var item = {
        ID: 0,
        Start: new Date().toISOString(),
        End: new Date().toISOString(),
        Description: "New work slot",
        Project: "Unspecified"
      };
      return (
<div>
  <div className="row">
    <div className="col-sm-3">&nbsp;</div>
    {filter}
  </div>
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
  <div className="row">
    <div className="col-sm-3">
      <button onClick={this.createSlot} className="btn btn-default btn-block">Add Time Slot</button>       
    </div>
    {filter}
  </div>
  <ul className="list-group">      
    {children}
  </ul>
</div>
      )   
    }
  }
}

export default ListSlots;
