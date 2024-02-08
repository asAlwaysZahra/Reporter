namespace Data.interfaces;

public interface IDataProvider
{
    IEnumerable<models.Task> GetData(); 
}