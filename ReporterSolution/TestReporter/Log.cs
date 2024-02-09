namespace TestReporter;

public class Log
{
    private static int _counter = 0;
    public int LogId { get; set; }
    public string Message { get; set; }
    public DateTime Time { get; set; }
    public bool IsError { get; set; } = false;

    public Log(string message, DateTime time, bool isError)
    {
        LogId = _counter++;
        Message = message;
        Time = time;
        IsError = isError;
    }
}
