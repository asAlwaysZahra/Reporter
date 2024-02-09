using System.Text.Json;

namespace DataProviderOne.DataReader;

public class JsonFileReader<T> : IFileReader<T>
{
    public List<T> ReadFile(string filePath)
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            var list = JsonSerializer.Deserialize<List<T>>(json);

            // chaeck if list is null
            return list ?? throw new ArgumentException("The path provided does not contain any data!");
        }
        else
        {
            throw new Exception("file not found!");
        }
    }
}
