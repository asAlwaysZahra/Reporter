using TestReporter.database;

namespace TestReporter;

public class Logger
{
    public List<Log> Logs { get; set; }

    public Logger()
    {
        using var context = new AppDbContext();
        Logs = [.. context.LogSet.ToList()];
    }

    public void LogMessage(Log log)
    {
        Logs.Add(log);
        using var context = new AppDbContext();
        context.LogSet.Add(log);
        context.SaveChanges();
    }
}
