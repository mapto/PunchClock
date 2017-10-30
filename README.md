# PunchClock

A mock Timesheet reporting project for an interview test. It corresponds to this extremely simple use case:
![Use-case diagram[https://github.com/mapto/PunchClock/raw/master/docs/use-case.png "The two simple required user stories, converted into use cases"].

Project developed with MonoDevelop Version 7.1 Preview (7.1 build 1291), [installed from flatpak](http://www.monodevelop.com/download/linux/). MVC, WebApi and NUnit included by default. 

Using [Mono.Data.Sqlite](http://www.mono-project.com/docs/database-access/providers/sqlite/#new-style-assembly-shipped-with-mono-124) as provider.

For the current configuration front-end (React at port 3000) and back-end (WebApi at port 8080) need to be run independently on the same machine via the ./run-*.sh files

## Architecture
![Structural diagram (Class and Component)[https://github.com/mapto/PunchClock/raw/master/docs/structural.png "Two class diagrams for front- and back-end, combined"]

### Back-end

Back-end dependencies activated in Mono Project:

* System.Data

* Mono.Data.Sqlite

* System.Runtime.Serialization

* Microsoft.AspNet.WebApi.Cors

### Front-end

Front-end dependencies ([using npm](https://www.npmjs.com/get-npm)):

* react react-dom

* react-bootstrap

### Database

Sqlite3 is used. A nice open source and multiplatform DB client that supports it is [DBeaver](https://dbeaver.jkiss.org).

### File hierarchy

In the main project directory CamelCased directories are part of the WebApi project. lowercased directories are part of the React project. The [docs][https://github.com/mapto/PunchClock/blob/master/docs/] contains the resources referenced in this description. The original user stories are omitted for interview anonymity reasons.

## Requests
The service interface is very simple. It allows getting items and creating/updating them.

###GET

Request:

    http://localhost:8080/api/Time

Response Content:

    [{"ID":36,"Start":"2018-08-03T09:00:00","End":"2018-08-03T18:00:00","Description":"Let me get a very long description to test how overflow is being handled. Viewing is fine, but expanding for editing will turn out to not have enough space. Let's see if it is really the case.","Project":"Unspecified"},{"ID":34,"Start":"2018-08-03T09:00:00","End":"2018-08-03T18:00:00","Description":"Set defaults for improved productivity","Project":"Interview"},{"ID":27,"Start":"2018-08-03T09:00:00","End":"2018-08-03T18:00:00","Description":"A third work slot","Project":"Unspecified"},{"ID":26,"Start":"2018-08-03T09:00:00","End":"2018-08-03T18:00:00","Description":"New work slot","Project":"Unspecified"},{"ID":24,"Start":"2018-08-03T10:00:00","End":"2018-08-03T19:00:00","Description":"Working, working","Project":"Test"},{"ID":18,"Start":"2018-08-03T10:00:00","End":"2018-08-03T19:00:00","Description":"Let's see if this makes it through","Project":"Test"},{"ID":7,"Start":"2018-07-03T09:00:00","End":"2018-07-03T18:00:00","Description":"Other task","Project":"Test"},{"ID":1,"Start":"2018-05-02T09:00:00","End":"2018-05-02T18:00:00","Description":"First task","Project":"Test"}]

###POST

Sample create request:

If ID already exists, that item is updated, otherwise a new item (without perserving the submitted ID) is created.

    http://localhost:8080/api/Time

Request Content:

    ID=36&Description=Let me get a very long description...&Start=2018-08-03T09:00:00&End=2018-08-03T18:00:00&Project=Test

HTTP response without content.

