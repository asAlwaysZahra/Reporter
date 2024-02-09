using ImplementationBase.models.enums;
using System.Data;
using Task = ImplementationBase.models.Task;

namespace Reporter;

public class TaskProcessor
{
    // Q1
    public static int CountTasks(IEnumerable<Task> tasks) => tasks.Count();

    // Q2
    public static int CountCompletedTasks(IEnumerable<Task> tasks) => tasks.Where(t => t.Done).Count();

    // Q3
    public static IEnumerable<Task> DoneBetween(IEnumerable<Task> tasks, DateTime t1, DateTime t2)
                    => tasks
                    .Where(t => t.DoneAt.CompareTo(t1) > 0 && t.DoneAt.CompareTo(t2) < 0)
                    .ToList();

    // Q4
    public static IEnumerable<Task> GetUncopmpletedTasks(IEnumerable<Task> tasks) => tasks
                    .Where(t => t.Done == false)
                    .ToList();

    // Q5
    public static IEnumerable<Task> GetOverduedTasks(IEnumerable<Task> tasks) => tasks
                    .Where(t => t.Deadline.CompareTo(DateTime.Now) < 0)
                    .ToList();

    // Q6
    public static IEnumerable<Task> Get3CreatedMoreThan5Days(IEnumerable<Task> tasks) => tasks
                    .Where(t => (DateTime.Now - t.CreationDate).TotalDays > 5 && !t.Done)
                    .OrderByDescending(t => t.CreationDate)
                    .Take(3)
                    .ToList();

    // Q7
    public static IEnumerable<Task> Get3DoneAtDayCreated(IEnumerable<Task> tasks) => tasks
                    .Where(t => (t.DoneAt.DayOfYear == t.CreationDate.DayOfYear && t.Done))
                    .OrderByDescending(t => t.CreationDate)
                    .Take(3)
                    .ToList();

    // Q8
    public static IEnumerable<Task> GetUncopmletedTasksByPriority(IEnumerable<Task> tasks, Priority pr) => tasks
                    .Where(t => t.Priority == pr && !t.Done)
                    .ToList();

}