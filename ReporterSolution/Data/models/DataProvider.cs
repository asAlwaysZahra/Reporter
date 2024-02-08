using Data.interfaces;
using Data.models.enums;

namespace Data.models;

public class DataProvider : IDataProvider
{
    public IEnumerable<Task> GetData()
    {
        Task task1 = new("AA task", "a task with same deadline and priority as task 2 and different creation date", DateTime.Now.AddDays(2), Priority.LowPriority);
        Task task2 = new("A task", "another task with same deadline as task 3 and less priority", DateTime.Now.AddDays(2), Priority.LowPriority);
        Task task3 = new("Clean your laptop", "its about 1 month you have not cleaned your laptop!", DateTime.Now.AddDays(2), Priority.HighPriority);
        Task task4 = new("Go to the dentist", "your theeth need to be checked", DateTime.Now.AddDays(7), Priority.LowPriority);
        Task task5 = new("DB homework", "the final project, phase 2", DateTime.Now.AddDays(3), Priority.Neutral);
        Task task6 = new("Reconcile eith your friend", "you know the right thing to do", DateTime.Now.AddDays(1), Priority.Critical);
        task1.CreationDate = DateTime.Now.AddDays(-1);

        List<Task> myList = new();
        myList.Add(task1);
        myList.Add(task2);
        myList.Add(task3);
        myList.Add(task4);
        myList.Add(task5);
        myList.Add(task6);

        return myList;
    }
}
