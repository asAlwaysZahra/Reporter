using Reporter.enums;
using System.Data;

namespace Reporter;

public class TaskProcessor
{
    // Q1
    public int CountTasks(IEnumerable<Task> tasks) => tasks.Count();

    // Q2
    public int CountCompletedTasks(IEnumerable<Task> tasks) => tasks.Where(t => t.Done).Count();

    // Q3

    public IEnumerable<Task> DoneBetween(IEnumerable<Task> tasks, DateTime t1, DataSetDateTime t2)
                    => tasks
                    .Where(t => t.DoneAt.CompareTo(t1) > 0 && t.DoneAt.CompareTo(t2) < 0);

    // Q4
    public IEnumerable<Task> GetUncopmpletedTasks(IEnumerable<Task> tasks) => tasks
                    .Where(t => t.Done == false);

    // Q5
    public IEnumerable<Task> GetOverduedTasks(IEnumerable<Task> tasks) => tasks
                    .Where(t => t.Deadline.CompareTo(DateTime.Now) < 0);

    // Q6

    public IEnumerable<Task> Get3CreatedMoreThan5Days(IEnumerable<Task> tasks) => tasks
                    .Where(t => (DateTime.Now - t.CreationDate).TotalDays > 5 && !t.Done)
                    .OrderByDescending(t => t.CreationDate)
                    .Take(3);

    // Q7
    public IEnumerable<Task> Get3DoneAtDayCreated(IEnumerable<Task> tasks) => tasks
                    .Where(t => (t.DoneAt.DayOfYear == t.CreationDate.DayOfYear && t.Done))
                    .OrderByDescending(t => t.CreationDate)
                    .Take(3);

    // Q8
    public IEnumerable<Task> GetUncopmletedTasksByPriority(IEnumerable<Task> tasks, Priority pr) => tasks
                    .Where(t => t.Priority == pr && !t.Done);

}