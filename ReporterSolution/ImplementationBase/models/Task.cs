using ImplementationBase.models.enums;

namespace ImplementationBase.models;

public class Task : IComparable<Task>
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime Deadline { get; set; }
    public Priority Priority { get; set; }
    public bool Done { get; set; }
    public DateTime DoneAt { get; set; }
    public TaskCategory Category { get; set; }

    public Task(int id, string title, string description, DateTime creationDate,
        DateTime deadline, Priority priority, bool done, DateTime doneAt, TaskCategory category)
    {
        Id = id;
        Title = title;
        Description = description;
        CreationDate = creationDate;
        Deadline = deadline;
        Priority = priority;
        Done = done;
        DoneAt = doneAt;
        Category = category;
    }

    public Task(string title, string description, DateTime deadline, Priority priority)
    {
        Id = StaticValues.TaskCounter++;
        Title = title;
        Description = description;
        CreationDate = DateTime.Now;
        Deadline = deadline;
        Priority = priority;
        Done = false;
        Category = 0;
    }

    public int CompareTo(Task? other)
    {
        ArgumentNullException.ThrowIfNull(other);

        if (!Deadline.ToString().Equals(other.Deadline.ToString()))
            return Deadline.CompareTo(other.Deadline);

        if (!Priority.Equals(other.Priority))
            return Priority.CompareTo(other.Priority);

        // ok, now the task that has been created earlier, has more priority
        return CreationDate.CompareTo(other.CreationDate);
    }

    public override string ToString()
        => $"\n{Id} | {Priority} | {Title} | Due: {Deadline}\n  - {Description}\n" +
           $"    Created at: {CreationDate}\n  Group: {Category}\n";
}
