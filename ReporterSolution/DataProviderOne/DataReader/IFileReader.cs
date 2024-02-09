namespace DataProviderOne.DataReader;

public interface IFileReader<T>
{
    List<T> ReadFile(string filePath);
}
