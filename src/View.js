import React, { Component } from 'react';

var getDuration = function(millisec) {
  // in hours, up to 15 mins accuracy
  return Math.round(Math.abs(millisec / 9e5))/4;
}

class SlotView extends Component {
  render() {
  	var startTimestamp = new Date(this.props.data.Start);
  	var endTimestamp = new Date(this.props.data.End);
   	var duration = getDuration(endTimestamp - startTimestamp);
    return (
 <div>
    <h3 className="list-group-item-heading">{this.props.data.ID}. {this.props.data.Description}</h3>
    <p className="list-group-item-text">{duration} hours on {startTimestamp.toDateString()} ({startTimestamp.toLocaleTimeString()} to {endTimestamp.toLocaleTimeString()})</p>
    <p className="list-group-item-text">Project: {this.props.data.Project}</p>
</div>
    );      
  }
}


export default SlotView;
