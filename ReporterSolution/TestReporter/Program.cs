// in the name of God;
using ImplementationBase.interfaces;
using ImplementationBase.models.enums;
using System.Reflection;
using Task = ImplementationBase.models.Task;


(Assembly reporterDll, List<Assembly> dataDll) = LoadDlls();

//var providerType = dataDll.GetTypes().Where(t => t.GetInterface("IDataProvider") != null).First();

Dictionary<Assembly, List<Task>> allData = [];

foreach (var assm in dataDll)
{
    var providerType = assm.GetTypes().Where(t => t.GetInterface("IDataProvider") != null).First();
    var getDataMethod = providerType.GetMethod("GetData");
    var data = (List<Task>)getDataMethod.Invoke(Activator.CreateInstance(providerType), null);

    allData.Add(assm, data);
}

List<TaskCategory> categories = FindCategories(allData);

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
                RunReport(input, allData, reporterDll);
            }
            else break;

            break;
        case "2":
            Dictionary<Assembly, bool> assemblies = [];
            ExtensionManagement(assemblies);
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
        var assem = Assembly.LoadFile(Path.Combine(Directory.GetCurrentDirectory(), file));

        bool o = assem
                .GetTypes()
                .Where(t => typeof(IDataProvider).IsAssignableFrom(t) && !t.IsInterface)
                .Any();

        if (o) dataAssemblies.Add(assem);
    }

    //dllFile = new FileInfo(@".\plugins\DataProviderOne.dll");
    //var dataDll = Assembly.LoadFile(dllFile.FullName);

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

static void ExtensionManagement(Dictionary<Assembly, bool> assemblies)
{
    Console.WriteLine("Manage Extensions:\n");

    Console.WriteLine("   Extension   |   State   ");
    Console.WriteLine("---------------|-----------");

    foreach (var assm in assemblies.Keys)
    {
        string state = assemblies[assm] ? "Enable" : "Disable";
        Console.WriteLine(assm.GetName() + "  |  " + state);
    }

    Console.WriteLine("\n1. Change State of Extension");
    Console.WriteLine("\n2. Back");

    string input = Console.ReadLine();

    switch (input)
    {
        case "1":
            Console.WriteLine("Enter name of extension:");
            input = Console.ReadLine();
            ChangeExtensionState(input);
            break;
        case "2":
            return;
            break;
        default:
            break;
    }

}

static void ChangeExtensionState(string name)
{
    throw new NotImplementedException();
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
