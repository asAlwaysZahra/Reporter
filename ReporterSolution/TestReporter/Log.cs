using System.ComponentModel.DataAnnotations.Schema;

namespace TestReporter;

public class Log
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public string Message { get; set; }
    public DateTime Time { get; set; }
    public bool IsError { get; set; } = false;

    public Log(string message, DateTime time, bool isError)
    {
        Message = message;
        Time = time;
        IsError = isError;
    }
}
