// in the name of God;
using ImplementationBase.models.enums;
using TestReporter;

class Program
{
    static void Main(string[] args)
    {
        Reporter reporter = new();

        // this method finds all different categories of tasks (TaskCategory is an enum)
        List<TaskCategory> categories = reporter.FindCategories();

        // main menu starts here
        while (true)
        {
            Menu.ShowMenu();
            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    Menu.ShowCategories(categories);

                    // select category:
                    input = Console.ReadLine();

                    // 0 shows exit
                    if (input != "0")
                    {
                        Menu.ShowReports();

                        // select report:
                        input = Console.ReadLine();

                        // run report
                        reporter.RunReport(input);
                    }
                    else break;

                    break;
                case "2":
                    reporter.ExtensionManagement();
                    break;
                case "3":
                    reporter.ShowLogHistory();
                    break;
                case "4":
                    break;
                default:
                    Console.WriteLine("Please select a valid option!");
                    break;
            }

            // exit program
            if (input == "4")
                return;
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
