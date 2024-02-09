using ImplementationBase.interfaces;
using ImplementationBase.models.enums;
using System.Reflection;
using ImplementationBase.models;
using Task = ImplementationBase.models.Task;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TestReporter;

public class Reporter
{
    public Assembly ReporterDll { get; set; }
    // to maintain all data .dll files and thier data (which is a list of tasks):
    public Dictionary<Assembly, List<Task>> AllExtensions { get; set; }
    public Dictionary<Assembly, List<Task>> ActiveExtensions { get; set; }
    public List<string> Logs { get; set; }

    public Reporter()
    {
        AllExtensions = new();
        ActiveExtensions = new();
        Logs = new();
        LoadDlls();
    }

    public void ShowLogHistory()
    {
        Console.WriteLine("History:\n");

        foreach (var log in Logs)
            Console.WriteLine(log);
    }

    public void LoadDlls()
    {
        var dllFile = new FileInfo(@".\plugins\Reporter.dll");
        ReporterDll = Assembly.LoadFile(dllFile.FullName);

        // read dll files from plugins folder
        var files = Directory.GetFiles(@".\plugins", "*.dll");

        // read assembly
        foreach (var file in files)
        {
            var assm = Assembly.LoadFile(Path.Combine(Directory.GetCurrentDirectory(), file));

            // just the ones that has implemented IDataProvider
            if (IsValidDataDll(assm))
                AllExtensions.Add(assm, []);
        }

        // fill data with tasks that each dll provides
        CollectDataDll();
    }

    private void CollectDataDll()
    {
        // collect all data provided by GetData method (declared in IDataProvider interface)
        foreach (var dataDll in AllExtensions.Keys)
        {
            var providerType = dataDll.GetTypes().Where(t => t.GetInterface("IDataProvider") != null).First();
            var getDataMethod = providerType.GetMethod("GetData");
            var data = (List<Task>)getDataMethod.Invoke(Activator.CreateInstance(providerType), null);

            // update tasks list for related assembly
            AllExtensions[dataDll] = data;
        }

        // I assume that at the begining, all extensions are enabeld
        ActiveExtensions = new Dictionary<Assembly, List<Task>>(AllExtensions);
    }

    public List<TaskCategory> FindCategories()
    {
        ISet<TaskCategory> categories = new HashSet<TaskCategory>();

        foreach (var list in ActiveExtensions.Values)
        {
            foreach (var task in list)
            {
                categories.Add(task.Category);
            }
        }

        return [.. categories];
    }

    public void RunReport(string input, TaskCategory selectedCategory)
    {
        Console.WriteLine("\n=== Bootcamp Reporter :: An extendible command-line report tool ===\n");

        if (EmptyActiveExtensions())
        {
            Console.WriteLine("Sorry, you can not run any report because there is no ENABLE extensions!");
            return;
        }

        // get the TaskProcessor calss that contains query methods
        var processor = ReporterDll.GetType("Reporter.TaskProcessor");
        var queryMethods = processor.GetMethods();


        string methodName = FindMethodAndArgs(input, out DateTime t1, out DateTime t2);

        if (methodName == "")
        {
            Console.WriteLine("Please select one option!");
            return;
        }

        // first, log the report
        LogReports(methodName);

        List<List<Task>> result = [];

        int i = 0, sumResult = 0;
        List<List<Task>> allExtensionsResult = [];

        Dictionary<Assembly, List<Task>> filtered = FilterByCategory(selectedCategory);

        foreach (var taskList in filtered.Values)
        {
            // find method
            MethodInfo methodInfo = queryMethods.Where(q => q.Name == methodName).First();

            // set arguments
            object[] argg = methodName == "DoneBetween" ? argg = [taskList, t1, t2] : argg = [taskList];

            // call method
            var returnValue = methodInfo.Invoke(null, argg);

            // print result
            string assmName = filtered.Keys.ToArray()[i++].FullName
                                                   .Split(" ")[0]
                                                   .Replace(",", "");

            Console.Write($"Result of '{methodName}' : On '{assmName}'  =>  ");

            if (returnValue is int r)
            {
                if (r == 0)
                {
                    Console.WriteLine("There is no task with the spicified conditions!");
                }
                else
                {
                    Console.WriteLine(returnValue);
                    sumResult += r;
                }
            }
            else if (returnValue is List<Task> res)
            {
                if (res.Count == 0)
                {
                    Console.WriteLine("There is no task with the spicified conditions!");
                }
                else
                {
                    allExtensionsResult.Add(res);

                    Console.WriteLine("\nTasks Count: " + res.Count);
                    Console.WriteLine("Tasks List: ");

                    foreach (var task in res)
                        Console.WriteLine(task);
                }
            }

            Console.WriteLine("~~~~");
        }

        int allCount;

        if (DetectReturnType(methodName, queryMethods) == ReturnType.List)
        {
            // flat all results and count them
            allCount = allExtensionsResult.SelectMany(t => t).Count();
        }
        else
        {
            allCount = sumResult;
        }

        Console.WriteLine($"\n~~ Count Result of All Enable Extensions = {allCount} ~~\n");
    }

    private Dictionary<Assembly, List<Task>> FilterByCategory(TaskCategory selectedCategory)
    {
        Dictionary<Assembly, List<Task>> filtered = [];

        foreach (var key in ActiveExtensions.Keys)
        {
            var tasks = ActiveExtensions[key];

            var filteredTasks = tasks
                                .Where(t => t.Category.Equals(selectedCategory))
                                .ToList();

            filtered.Add(key, filteredTasks);
        }

        return filtered;
    }

    private ReturnType DetectReturnType(string methodName, MethodInfo[] queryMethods)
    {
        // find method
        MethodInfo methodInfo = queryMethods.Where(q => q.Name == methodName).First();

        if (methodInfo.ReturnType == typeof(int))
            return ReturnType.Int;
        else
            return ReturnType.List;
    }

    public void ExtensionManagement()
    {
        Console.WriteLine("\nManage Extensions:\n");

        Console.WriteLine("    Extension     |   State   ");
        Console.WriteLine("------------------|-----------");

        foreach (var assm in AllExtensions.Keys)
        {
            string state = ActiveExtensions.ContainsKey(assm) ? "Enable" : "Disable";
            string name = assm.FullName.Split(" ").ToArray()[0].Replace(",", "");

            Console.WriteLine(name + "       " + state);
        }

        // show options in extension maangement part
        Console.WriteLine("\n1. Change State of Extension");
        Console.WriteLine("\n2. Add New Extension");
        Console.WriteLine("\n3. Back");

        // get user choice
        string input = Console.ReadLine();

        switch (input)
        {
            case "1":
                Console.Write("Enter name of extension: ");
                input = Console.ReadLine();

                bool success = ChangeExtensionState(input);

                if (success)
                    Console.WriteLine("\nState Changed Successfully!");
                else
                    Console.WriteLine("\nSomething went wrong in changing the state :(");

                break;
            case "2":
                AddNewExtension();
                break;
            case "3":
                return;
                break;
            default:
                Console.WriteLine("Wrong Input!");
                break;
        }

    }

    public bool ChangeExtensionState(string name)
    {
        bool found = false;

        foreach (var assm in AllExtensions.Keys)
        {
            if (assm.FullName.StartsWith(name))
            {
                if (ActiveExtensions.ContainsKey(assm))
                {
                    ActiveExtensions.Remove(assm);
                }
                else
                {
                    ActiveExtensions.Add(assm, AllExtensions[assm]);
                }

                found = true;
                break;
            }
        }

        return found;
    }

    public void AddNewExtension()
    {

        Console.WriteLine("* (Please notice that your file must be located in 'plugins' folder");
        Console.WriteLine("   and also it should be implemented 'IDataProvider' interface!) *");

        Console.Write("Enter the name of your .dll file:");

        string name = Console.ReadLine();

        var dllFile = new FileInfo(@$".\plugins\{name}.dll");
        var assm = Assembly.LoadFile(dllFile.FullName);

        var data = CollectAssemblyTasks(assm);

        ActiveExtensions.Add(assm, data);
    }

    public List<Task> CollectAssemblyTasks(Assembly assm)
    {

        if (IsValidDataDll(assm))
        {
            var providerType = assm.GetTypes().Where(t => t.GetInterface("IDataProvider") != null).First();

            var getDataMethod = providerType.GetMethod("GetData");

            var data = (List<Task>)getDataMethod.Invoke(Activator.CreateInstance(providerType), null);

            return data;
        }
        else
        {
            throw new ArgumentException("Something went wrong in collecting assembly tasks data!");
        }
    }

    public bool IsValidDataDll(Assembly assembly)
             => assembly
                .GetTypes()
                .Where(t => typeof(IDataProvider).IsAssignableFrom(t) && !t.IsInterface)
                .Any();

    public void LogReports(string operation)
    {
        Logs.Add($"Run '{operation}' on {DateTime.Now}");
    }

    public string FindMethodAndArgs(string input, out DateTime t1, out DateTime t2)
    {
        t1 = DateTime.Now;
        t2 = DateTime.Now;
        string methodName = "";

        switch (input)
        {
            case "1":
                methodName = "CountTasks";
                break;

            case "2":
                methodName = "CountCompletedTasks";
                break;
            case "3":
                methodName = "DoneBetween";

                Console.WriteLine("Please enter dates in format of <MM/dd/yyyy hh:mm:ss>");

                Console.Write("- first date:  ");
                string t = Console.ReadLine();
                t1 = DateTime.Parse(t);

                Console.Write("- secons date:  ");
                t = Console.ReadLine();
                t2 = DateTime.Parse(t);

                break;
            case "4":
                methodName = "GetUncopmpletedTasks";
                break;
            case "5":
                methodName = "GetOverduedTasks";
                break;
            case "6":
                methodName = "Get3CreatedMoreThan5Days";
                break;
            case "7":
                methodName = "Get3DoneAtDayCreated";
                break;
            case "8":
                methodName = "GetUncopmletedTasksByPriority";
                break;
            default:
                Console.Error.WriteLine("Please select a valid option :/");
                break;
        }

        return methodName;
    }

    private bool EmptyActiveExtensions()
    {
        bool found = false;

        foreach (var tasks in ActiveExtensions.Values)
            if (tasks.Count > 0)
            {
                found = true;
                break;
            }

        return !found;
    }

    enum ReturnType
    {
        Int,
        List
    }
}
