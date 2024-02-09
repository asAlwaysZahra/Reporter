// in the name of God;
using ImplementationBase.models.enums;
using TestReporter;

class Program
{
    static void Main(string[] args)
    {
        // first lets compare load times in normal and parallel ways :D
        CompareLoadWays();

        // all aperation is done in this class
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

                    if (Convert.ToInt32(input) > categories.Count)
                    {
                        Console.WriteLine("Invalid category!");
                    }
                    else
                    {
                        // 0 shows back
                        if (input != "0")
                        {
                            TaskCategory selectedCategory = categories[Convert.ToInt32(input) - 1];

                            // show report options
                            Menu.ShowReports();

                            // select report:
                            input = Console.ReadLine();

                            // run report
                            reporter.RunReport(input, selectedCategory);
                        }
                    }
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

    static void CompareLoadWays()
    {
        Reporter reporter = new();

        reporter.AllExtensions.Clear();
        DateTime start = DateTime.Now;
        reporter.LoadDlls();
        DateTime end = DateTime.Now;
        Console.WriteLine("normal: " + end.Subtract(start).TotalMilliseconds + " ms");

        reporter.AllExtensions.Clear();
        start = DateTime.Now;
        reporter.LoadParallel(reporter.DivideFiles());
        end = DateTime.Now;
        Console.WriteLine("parallel: " + end.Subtract(start).TotalMilliseconds + " ms");

        Console.WriteLine("Press any key to continue..");
        Console.ReadLine();
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
