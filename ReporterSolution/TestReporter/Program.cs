// in the name of God;
using ImplementationBase.interfaces;
using ImplementationBase.models.enums;
using System.Reflection;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Task = ImplementationBase.models.Task;


(Assembly reporterDll, List<Assembly> dataDll) = LoadDlls();

// to maintain all data dll files and thier data (list of tasks)
Dictionary<Assembly, List<Task>> allData = [];

// collect all data provided by GetData method (declared in IDataProvider interface)
foreach (var assm in dataDll)
{
    var providerType = assm.GetTypes().Where(t => t.GetInterface("IDataProvider") != null).First();
    var getDataMethod = providerType.GetMethod("GetData");
    var data = (List<Task>)getDataMethod.Invoke(Activator.CreateInstance(providerType), null);

    allData.Add(assm, data);
}

// I assume that at the begining, all extensions are enabeld
Dictionary<Assembly, List<Task>> activeData = new Dictionary<Assembly, List<Task>>(allData);

// this method finds all different categories of tasks (TaskCategory is an enum)
List<TaskCategory> categories = FindCategories(activeData);

// main menu
while (true)
{
    ShowMenu();
    string input = Console.ReadLine();

    switch (input)
    {
        case "1":
            ShowCategories(categories);

            // select category:
            input = Console.ReadLine();

            // 0 shows exit
            if (input != "0")
            {
                ShowReports();

                // select report:
                input = Console.ReadLine();

                // run report
                RunReport(input, activeData, reporterDll);
            }
            else break;

            break;
        case "2":
            ExtensionManagement(ref activeData, ref activeData);
            break;
        case "3":
            break;
        default:
            break;
    }

    if (input == "4")
        break;
}

static (Assembly, List<Assembly>) LoadDlls()
{
    var dllFile = new FileInfo(@".\plugins\Reporter.dll");
    var reporterDll = Assembly.LoadFile(dllFile.FullName);

    // read dll files from plugins folder
    var files = Directory.GetFiles(@".\plugins", "*.dll");

    // read assembly
    List<Assembly> dataAssemblies = [];

    foreach (var file in files)
    {
        var assm = Assembly.LoadFile(Path.Combine(Directory.GetCurrentDirectory(), file));
        if (IsValidDataDll(assm)) dataAssemblies.Add(assm);
    }

    return (reporterDll, dataAssemblies);
}

static void ShowMenu()
{
    Console.WriteLine("\n=== Bootcamp Reporter :: An extendible command-line report tool ===\n");
    Console.WriteLine("1. Run Report");
    Console.WriteLine("2. Manage Extensions");
    Console.WriteLine("3. Report History\n");

    Console.WriteLine("4. Exit");
}

static void ShowCategories(List<TaskCategory> set)
{
    Console.WriteLine("\n=== Bootcamp Reporter :: An extendible command-line report tool ===\n");

    Console.WriteLine("- Select Category:");

    int i = 1;
    foreach (var category in set)
        Console.WriteLine($"{i++}. {category}");

    Console.WriteLine("\n0. Back");
}

static List<TaskCategory> FindCategories(Dictionary<Assembly, List<Task>> allTasks)
{
    ISet<TaskCategory> categories = new HashSet<TaskCategory>();

    foreach (var list in allTasks.Values)
    {
        foreach (var task in list)
        {
            categories.Add(task.Category);
        }
    }

    return [.. categories];
}

static void ShowReports()
{
    Console.WriteLine("\n=== Bootcamp Reporter :: An extendible command-line report tool ===\n");

    Console.WriteLine("- Select Report:");
    Console.WriteLine("1. Count of Tasks");
    Console.WriteLine("2. Count of Completed Tasks");
    Console.WriteLine("3. Task Done Between Dates");
    Console.WriteLine("4. Uncopmpleted Tasks");
    Console.WriteLine("5. Overdued Tasks");
    Console.WriteLine("6. Last 3 Uncompleted Tasks; Created More Than 5 Days");
    Console.WriteLine("7. Last 3 Tasks Done At Day Created");
    Console.WriteLine("8. Uncopmleted Tasks By Priority");

    Console.WriteLine("\n9. Back");
}

static void RunReport(string input, Dictionary<Assembly, List<Task>> tasks, Assembly reporter)
{
    Console.WriteLine("\n=== Bootcamp Reporter :: An extendible command-line report tool ===\n");

    var processor = reporter.GetType("Reporter.TaskProcessor");
    var queryMethods = processor.GetMethods();

    DateTime t1 = DateTime.Now, t2 = DateTime.Now;
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

            string t = Console.ReadLine();
            t1 = DateTime.Parse(t);

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

    if (methodName == "")
    {
        Console.WriteLine("Please select one option!");
        return;
    }

    List<List<Task>> result = [];

    int i = 0;
    foreach (var list in tasks.Values)
    {
        // find method
        MethodInfo methodInfo = queryMethods.Where(q => q.Name == methodName).First();

        // set arguments
        object[] argg = (methodName == "DoneBetween") ? argg = [list, t1, t2] : argg = [list];

        // call method
        var returnValue = methodInfo.Invoke(null, argg);

        // print result
        string assmName = tasks.Keys.ToArray()[i++].FullName.Split(" ")[0].Replace(",", "");

        Console.Write($"Result of '{methodName}' : On '{assmName}'  =>  ");

        if (returnValue is int)
        {
            Console.WriteLine(returnValue + "\n");
        }
        else if (returnValue is List<Task> res)
        {
            Console.WriteLine("\nTasks Count: " + res.Count);
            Console.WriteLine("Tasks List: ");

            foreach (var task in res)
                Console.WriteLine(task);
        }

        Console.WriteLine("~~~~~~~~~");
    }
}

static void ExtensionManagement(ref Dictionary<Assembly, List<Task>> allData, 
                                ref Dictionary<Assembly, List<Task>> actives)
{
    Console.WriteLine("Manage Extensions:\n");

    Console.WriteLine("   Extension   |   State   ");
    Console.WriteLine("---------------|-----------");

    foreach (var assm in allData.Keys)
    {
        string state = (actives.ContainsKey(assm)) ? "Enable" : "Disable";
        Console.WriteLine(assm.GetName() + "  |  " + state);
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
            Console.WriteLine("Enter name of extension:");
            input = Console.ReadLine();

            bool success = ChangeExtensionState(allData, ref actives, input);

            if (success)
                Console.WriteLine("State Changed Successfully!");
            else
                Console.WriteLine("Something went wrong in changing the state :(");

            break;
        case "2":
            AddNewExtension(ref allData);
            break;
        case "3":
            return;
            break;
        default:
            Console.WriteLine("Wrong Input!");
            break;
    }

}

static bool ChangeExtensionState(Dictionary<Assembly, List<Task>> allData, 
                                 ref Dictionary<Assembly, List<Task>> actives, 
                                 string name)
{
    bool found = false;

    foreach (var assm in allData.Keys)
    {
        if (assm.FullName.StartsWith(name) && !actives.ContainsKey(assm))
        {
            actives.Add(assm, allData[assm]);
            found = true;
            break;
        }
    }

    return found;
}

static void AddNewExtension(ref Dictionary<Assembly, List<Task>> allData)
{

    Console.WriteLine("* (Please notice that your file must be located in 'plugins' folder");
    Console.WriteLine("   and also it should be implemented 'IDataProvider' interface!) *");

    Console.Write("Enter the name of your .dll file:");

    string name = Console.ReadLine();

    var dllFile = new FileInfo(@$".\plugins\{name}.dll");
    var assm = Assembly.LoadFile(dllFile.FullName);

    var data = CollectAssemblyTasks(assm);

    allData.Add(assm, data);
}

static List<Task> CollectAssemblyTasks(Assembly assm)
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

static bool IsValidDataDll(Assembly assm)
{
    return assm
            .GetTypes()
            .Where(t => typeof(IDataProvider).IsAssignableFrom(t) && !t.IsInterface)
            .Any();
}

//class EFContext : DbContext
//{
//    public DbSet<Task> Tasks;

//    protected override void OnConfiguring(DbContextOptionsBuilder builder)
//    {
//        builder.UseSqlServer(@"Data Source= Server=localhost\SQLEXPRESS;Database=master;Trusted_Connection=True;");
//    }
//}

//[AttributeUsage(AttributeTargets.Method, Inherited = false)]
//class ReportExtensionAttribute : Attribute
//{
//    public string Message { get; set; }

//    public ReportExtensionAttribute(string message)
//    {
//        Message = message;
//    }
//}

//int n = 20;

//if (n > 10)
//{
//    int m = n % 10;
//    for (int i = 0; i < m; i++)
//    {
//        List<int> list = new List<int>();
//        for (int j = 0; j < 10; j++)
//        {
//            list.Add(j);
//        }
//    }
//}
