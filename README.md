# PunchClock

A mock Timesheet reporting project for an interview test.

Project developed with MonoDevelop Version 7.1 Preview (7.1 build 1291), [installed from flatpak](http://www.monodevelop.com/download/linux/). MVC, WebApi and NUnit included by default. 

Using [Mono.Data.Sqlite](http://www.mono-project.com/docs/database-access/providers/sqlite/#new-style-assembly-shipped-with-mono-124) as provider.

For the current configuration front-end (React at port 3000) and back-end (WebApi at port 8080) need to be run independently on the same machine via the ./run-*.sh files

## Back-end

Back-end dependencies activated in Mono Project:

* System.Data

* Mono.Data.Sqlite

* System.Runtime.Serialization

* Microsoft.AspNet.WebApi.Cors

## Front-end

Front-end dependencies ([using npm](https://www.npmjs.com/get-npm)):

* react react-dom

* react-bootstrap

## Database

Sqlite3 is used. A nice open source and multiplatform DB client that supports it is [DBeaver](https://dbeaver.jkiss.org).

## File hierarchy

In the main project directory CamelCased directories are part of the WebApi project, lowercased directories are part of the React project.
