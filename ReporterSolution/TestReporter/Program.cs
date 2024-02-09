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
                    string cat = Console.ReadLine();

                    if (Convert.ToInt32(cat) > categories.Count)
                    {
                        Console.WriteLine("Invalid category!");
                    }
                    else
                    {
                        // 0 shows back
                        if (cat != "0")
                        {
                            TaskCategory selectedCategory = categories[Convert.ToInt32(cat) - 1];

                            // show report options
                            Menu.ShowReports();

                            // select report:
                            string rep = Console.ReadLine();

                            // run report
                            reporter.RunReport(rep, selectedCategory);
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
        Console.WriteLine("first lets compare load times in normal and parallel ways :D");

        Reporter reporter = new();

        reporter.AllExtensions.Clear();

        DateTime start = DateTime.Now;
        reporter.LoadDlls();
        DateTime end = DateTime.Now;
        double timeSpent1 = end.Subtract(start).TotalMilliseconds;
        Console.WriteLine($"normal: {timeSpent1} ms");

        // -------------------------------------------------------

        reporter.AllExtensions.Clear();

        start = DateTime.Now;
        reporter.LoadParallel();
        end = DateTime.Now;
        double timeSpent2 = end.Subtract(start).TotalMilliseconds;
        Console.WriteLine($"parallel: {timeSpent2} ms");

        Console.WriteLine($"Diff = {timeSpent2 - timeSpent1} ms");

        Console.WriteLine("\nPress enter key to continue..");
        Console.ReadLine();
    }
}