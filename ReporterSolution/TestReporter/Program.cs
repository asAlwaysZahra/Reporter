using System.Reflection;
using Task = ImplementationBase.models.Task;


Console.WriteLine("Hello, World!");

var dllFile = new FileInfo(@".\plugins\Reporter.dll");
var reporterDll = Assembly.LoadFile(dllFile.FullName);

dllFile = new FileInfo(@".\plugins\DataProviderOne.dll");
var dataDll = Assembly.LoadFile(dllFile.FullName);


var type = dataDll.GetTypes().Where(t => t.GetInterface("IDataProvider") != null).First();
//var tasktype = dataDll.GetTypes().Where(t => t.Name == "Task").First();
var mi = type.GetMethod("GetData");

var res = (List<Task>)mi.Invoke(Activator.CreateInstance(type), null);


var processor = reporterDll.GetType("Reporter.TaskProcessor");
var queryMethods = processor.GetMethods();

string q = "Count";

if (q == "Count" && res != null)
{
    object[] arg = { res };
    var resQ = queryMethods.Where(q => q.Name == "CountTasks").First().Invoke(null, arg);
}

int n = 20;

if (n > 10)
{
    int m = n % 10;
    for (int i = 0; i < m; i++)
    {
        List<int> list = new List<int>();
        for (int j = 0; j < 10; j++)
        {
            list.Add(j);
        }
    }
}

//class EFContext : DbContext
//{
//    public DbSet<Task> Tasks;

//    protected override void OnConfiguring(DbContextOptionsBuilder builder)
//    {
//        builder.UseSqlServer(@"Data Source= Server=localhost\SQLEXPRESS;Database=master;Trusted_Connection=True;");
//    }
//}

[AttributeUsage(AttributeTargets.Method, Inherited = false)]
class ReportExtensionAttribute : Attribute
{
    public string Message { get; set; }

    public ReportExtensionAttribute(string message)
    {
        Message = message;
    }
}
