import React, { Component } from 'react';

class SlotView extends Component {
  render() {
    return (
 <div>
    <h3 className="list-group-item-heading">{this.props.data.ID}. {this.props.data.Description}</h3>
    <p className="list-group-item-text">from {this.props.data.Start} to {this.props.data.End}</p>
    <p className="list-group-item-text">Project: {this.props.data.Project}</p>
</div>
    );      
  }
}


export default SlotView;
