namespace PassTheWord.Logging;

/// <summary>
/// Logger abstraction used to avoid direct Console.WriteLine calls throughout the application.
/// </summary>
public interface ILogger
{
    LogLevel Level { get; }

    void Warning(string message);
    void Info(string message);
}

