# Reporter
hi C# - 2

## How to run this project?

First opent the solution in Visual Studio and setup your sql server database (create a database named "ReporterLogsDB").

Then, copy both tasks.json and plugins in this path: TestReporter/bin/Debug/net8.0 . 
You can copy other .dll files with specified interface (in ImplementationBase project)
in this path and run program. (even at runtime, you can do this by the option provided: Manage Extensions > Add New Extension)

The startup project is TestReporter. You can run it and choose reports from menu to run on enable extensions.

Note: there are 2 different DataDll files: DataProviderOne and DataProviderTwo that you can see their projects. others(DataProviderThree, .. , DataProvider16) are just copy of DataProviderOne :] to test functionality of code.
