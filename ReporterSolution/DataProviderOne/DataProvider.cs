using DataProviderOne.DataReader;
using ImplementationBase.interfaces;
using Task = ImplementationBase.models.Task;

namespace DataProviderOne;

public class DataProvider : IDataProvider
{
    public IEnumerable<Task> GetData()
    {
        return ReadFile("tasks.json");
    }

    List<Task> ReadFile(string path)
    {
        var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        var absolutePath = Path.Combine(baseDirectory, path);

        IFileReader<Task> reader = new JsonFileReader<Task>();

        List<Task> tasks = reader.ReadFile(absolutePath);

        return tasks;
    }
}
