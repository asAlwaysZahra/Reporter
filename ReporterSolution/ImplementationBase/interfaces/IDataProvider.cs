namespace ImplementationBase.interfaces;

public interface IDataProvider
{
    IEnumerable<models.Task> GetData(); 
}