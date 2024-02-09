# Reporter
hi C# - 2

## How to run this project?

First open the solution in Visual Studio and setup your sql server database (create a database named "ReporterLogsDB").

Then, copy both tasks.json and plugins folder (as it is) in this path: TestReporter/bin/Debug/net8.0 .
The tasks.json file is needed for DataProviderTwo, it must be in the path specified.
You can copy other .dll files with specified interface (in ImplementationBase project)
in this path and run program. (even at runtime, you can do this by the option provided: Manage Extensions > Add New Extension)

The startup project is TestReporter. You can run it and choose reports from menu to run on enable extensions.

- Reporter project: this project is a class library that includes methods to run a report (using LinQ);
- ImplementationBase: a class library that contains interfaces that should be implementd by a data provider;
- DataProviders: class libraries that implement needed interfaces to provide data;
- TestReporter: a console project that uses DataProvider and Reporter dll files to run reports on tasks provided by DataProvider.


Note: there are 2 different DataDll files: DataProviderOne and DataProviderTwo that you can see their projects. others(DataProviderThree, .. , DataProvider16) are just copy of DataProviderOne :] to test functionality of code.
