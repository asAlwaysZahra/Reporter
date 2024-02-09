using ImplementationBase.attributes;
using System.Data;
using Task = ImplementationBase.models.Task;

namespace Reporter;

public class TaskProcessor
{
    // Q1
    [BootcampReport("CountTasks")]
    public static int CountTasks(IEnumerable<Task> tasks) => tasks.Count();

    // Q2
    [BootcampReport("CountCompletedTasks")]
    public static int CountCompletedTasks(IEnumerable<Task> tasks) => tasks.Where(t => t.Done).Count();

    // Q3
    [BootcampReport("DoneBetween")]
    public static IEnumerable<Task> DoneBetween(IEnumerable<Task> tasks, DateTime t1, DateTime t2)
                    => tasks
                    .Where(t => t.DoneAt.CompareTo(t1) > 0 && t.DoneAt.CompareTo(t2) < 0)
                    .ToList();

    // Q4
    [BootcampReport("GetUncopmpletedTasks")]
    public static IEnumerable<Task> GetUncopmpletedTasks(IEnumerable<Task> tasks) => tasks
                    .Where(t => t.Done == false)
                    .ToList();

    // Q5
    [BootcampReport("GetOverduedTasks")]
    public static IEnumerable<Task> GetOverduedTasks(IEnumerable<Task> tasks) => tasks
                    .Where(t => t.Deadline.CompareTo(DateTime.Now) < 0)
                    .ToList();

    // Q6
    [BootcampReport("Get3CreatedMoreThan5Days")]
    public static IEnumerable<Task> Get3CreatedMoreThan5Days(IEnumerable<Task> tasks) => tasks
                    .Where(t => (DateTime.Now - t.CreationDate).TotalDays > 5 && !t.Done)
                    .OrderByDescending(t => t.CreationDate)
                    .Take(3)
                    .ToList();

    // Q7
    [BootcampReport("Get3DoneAtDayCreated")]
    public static IEnumerable<Task> Get3DoneAtDayCreated(IEnumerable<Task> tasks) => tasks
                    .Where(t => (t.DoneAt.DayOfYear == t.CreationDate.DayOfYear && t.Done))
                    .OrderByDescending(t => t.CreationDate)
                    .Take(3)
                    .ToList();

    // Q8
    [BootcampReport("GetUncopmletedTasksByPriority")]
    public static IEnumerable<Task> GetUncopmletedTasksByPriority(IEnumerable<Task> tasks) => tasks
                    .Where(t => !t.Done)
                    .OrderBy(t => t.Priority)
                    .ToList();
    
    // some dummy method without attribut (won't ne loaded)
    public static IEnumerable<Task> DoNotCallThis()
    {
        throw new NotImplementedException();
    }
}