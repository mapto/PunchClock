import React, { Component } from 'react';

class SlotModify extends Component {
  render() {
    var idx = this.props.data.ID ? this.props.data.ID : '#';
    return (
<form action="/api/Time/" method="POST" className="form-horizontal">
  <div className="col-sm-2">
    <input id="slotId" name="idx" disabled defaultValue={idx} type="text" className="form-control"/>
  </div>
  <label htmlFor="slotDescr" className="col-sm-2 control-label">Description</label>
  <div className="col-sm-8">  
    <input id="slotDescr" name="description" defaultValue={this.props.data.Description} type="text" className="form-control"/>
  </div>
  <label htmlFor="slotStart" className="col-sm-1 control-label">Start</label>
  <div className="col-sm-5">
    <input id="slotStart" name="start" defaultValue={this.props.data.Start} type="datetime-local" className="form-control"/>
  </div>
  <label htmlFor="slotStart" className="col-sm-1 control-label">End</label>
  <div className="col-sm-5">
  <input id="slotEnd" name="end" defaultValue={this.props.data.End} type="datetime-local" className="form-control"/>
  </div>
  <label htmlFor="slotProject" className="col-sm-1 control-label">Project</label>
  <div className="col-sm-11">
  <input id="slotProject" name="project" defaultValue={this.props.data.Project} type="text" className="form-control"/>
  </div>
</form>
    );      
  }
}

export default SlotModify;
