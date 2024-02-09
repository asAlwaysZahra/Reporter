namespace TestReporter;

public class Logger
{
    public List<Log> Logs { get; set; }

    public void LogMessage(Log log)
    {
        Logs.Add(log);

    }
}
