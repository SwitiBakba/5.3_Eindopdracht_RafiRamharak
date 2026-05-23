namespace PassTheWord.Logging;

public interface ILogger
{
    LogLevel Level { get; }

    void Warning(string message);
    void Info(string message);
}

