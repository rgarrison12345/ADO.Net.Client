# ADO.Net.Client
These libraries are a very thin wrapper around a given [ADO.NET](https://msdn.microsoft.com/en-us/library/e80y5yhx.aspx) client driver.
These libraries are capable of working with any ADO.Net compatible driver, meaining they are database agnostic.  There are three libraries that operate at different levels of the ADO.Net workflow, discussed below in the nuget packages section. 

## Why Use This Library?

### .NET Standard Support

This library supports .NET Framework 4.5+ and .NET Standard2.0+

### Database Agnostic

These libraries are not tied to any given database providers driver.  This means that you can focus on 
your SQL and not on the code that you would write.  This also encourages you to write your SQL
in a more standard fashion following the ANSI standard.

### Solves 90% Use Cases

These libraries acknowledge that all use case scenarios are unique
and they attempt to solve the 90% use cases out there

### Lightweight

This libraries make up a Micro-ORM, this is not a full blown framework like Entity.
Only the core of ADO.NET is implemented, not EF or Designer types.

### Difference between other Micro-ORMs

* Truly Database agnostic - There are Micro-ORMs that are compatabile with several ADO.Net client drivers.
  However they almost always implement some workarounds for bugs or attempt to implement functionality such as paging
  and limiting a result set in a standardized way in managed code that is not performed in the driver itself.  They
  are trying to standardized functionality that is found in most databases
  but the mechanism does not exist in standard ANSI-SQL.
  While these are good features, it's better to let the driver do this work because that is what it's built to do.
  These libraries contain no specific code that targets any specific providers client driver.  These libraries
  take the assumption that you are aware of your targeted drivers abilities as well as any idiosyncraices and bugs.
  The means is provided to work around anything you need to at any level if you have a specific driver implementation.

* Prepared Statements - Other Micro-ORMs do not allow the ability to prepare queries when needed.  Since these libraries give you
more control you can optionally prepare a query when necessary.  Queries can be prepared asynchronously
in .NET Standard 2.1 and above

* Assumed SQL skill level - Unlike other Micro-ORMs these libraries assume that you are
comftorable enough with SQL that you are able to write it without issue.  Peta-Poco and Dapper
have code that will automatically do an insert, update, delete statement based on a type passed in,
but this is taking control away from the developer.  The main benefit for that style of coding
is getting something together quickly, not necessarily maintainability.  

* Control - Most ORMs hide the entire ADO.Net workflow from the user.  From the creation of the objects
needed to query the database to the code that actually executes it from a command object.  These libraries 
give you the full power of the ADO.Net workflow that allows you to do what you need and not let another Micro-ORM
leave almost everything out of your hands.  This library does provide some default
implementations if you're not really interested in doing the hard work yourself.

* Smaller - These libraries are even smaller than other Micro-ORMs because they take the assumption
that you can write SQL and you know enough about ADO.Net that you could possibly if needed write some SQL yourself.

### Async Support

This library implements true asynchronous I/O for database operations, without blocking
(or using `Task.Run` to run synchronous methods on a background thread). This greatly
improves the throughput of a web server that performs database operations.

#### IAsyncEnumerable Support

This library is capable of returning a typed object as an async enumerable on read operations.  The IAsyncEnumerable interface was introduced in .NET Core 3.1 and C# 8.  This feature is available in .NET 4.6.1 and .NETStandard builds.

### Licensing

These libraries are licensed under the [MIT License](LICENSE.md) and may be freely distributed with commercial software.

## Goals

The goals of this project are:

* **.NET Standard support:** It must run on the full .NET Framework and all platforms supported by .NET Core.
* **Async:** All operations must be truly asynchronous whenever possible.
* **Lightweight:** Only the core of ADO.NET is implemented, not EF or Designer types.
* **Managed:** Managed code only, no native code.
* **Independent:** These libraries are not bound to any specific ADO.Net driver.

## NuGet Packages

* [ADO.Net.Client.Annotations](https://www.nuget.org/packages/ADO.Net.Client.Annotations/) is a library for annotations to properties on .NET objects 

* [ADO.Net.Client.Core](https://www.nuget.org/packages/ADO.Net.Client.Core/) is the lowest level library that contains the factory objecst that
creates the factory objects from an ADO.Net providers client drivers.  It also contains the abstract and interface objects that define higher level objects.
  This is a very thin wrapper around the actual client driver itself.
* [ADO.Net.Client.Implementation](https://www.nuget.org/packages/ADO.Net.Client.Implementation/) implements the interfaces and abstract classes
  contained in [ADO.Net.Client.Core](https://github.com/rgarrison12345/ADO.Net.Client/tree/master/src/ADO.Net.Client.Core).  This is the middle level library that directly consumes
  the core library.
* [ADO.Net.Client](https://www.nuget.org/packages/ADO.Net.Client/) is the highest level library that allows
  the consuming client code to allow the developer to focus on their SQL and encourages the developer to write more standard SQL
  and not follow properietary extensions unless necessary. This library consumes
  types found in [ADO.Net.Client.Implementation](https://www.nuget.org/packages/ADO.Net.Client.Implementation/)

## Contributing

If you'd like to contribute to ADO.Net.Client libraries, please read the [contributing guidelines](CONTRIBUTING.md).
