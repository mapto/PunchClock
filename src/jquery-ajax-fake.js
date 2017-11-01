var $ = {};
$.ajax = function(param) {
  var type = param.type || 'GET';
  var contentType = param.contentType || 'application/json; charset=UTF-8';

  var request = new XMLHttpRequest();
  request.open(type, param.url, true);
  request.setRequestHeader('Content-Type', contentType);
  request.onload = function() {
    if (request.status >= 200 && request.status < 400) {
      param.success(request.response);
    }
  }
  request.send(param.data);
}

export default $;

