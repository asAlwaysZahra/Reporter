using ImplementationBase.models.enums;

namespace TestReporter;

public class Menu
{
    public static void ShowMenu()
    {
        Console.WriteLine("\n=== Bootcamp Reporter :: An extendible command-line report tool ===\n");
        Console.WriteLine("1. Run Report");
        Console.WriteLine("2. Manage Extensions");
        Console.WriteLine("3. Report History\n");

        Console.WriteLine("4. Exit");
    }

    public static void ShowCategories(List<TaskCategory> set)
    {
        Console.WriteLine("\n=== Bootcamp Reporter :: An extendible command-line report tool ===\n");

        Console.WriteLine("- Select Category:");

        int i = 1;
        foreach (var category in set)
            Console.WriteLine($"{i++}. {category}");

        Console.WriteLine("\n0. Back");
    }

    public static void ShowReports()
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
}
