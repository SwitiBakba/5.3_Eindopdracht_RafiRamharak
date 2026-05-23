namespace PassTheWord.Logging;

public class ConsoleLogger : ILogger
{
    public LogLevel Level { get; }

    public ConsoleLogger(LogLevel logLevel)
    {
        Level = logLevel;
    }

    public void Warning(string message)
    {
        if (Level >= LogLevel.Warnings)
        {
            Console.ForegroundColor = ConsoleColor.Red;

            Console.WriteLine($"[WARNING] {message}");

            Console.ResetColor();
        }
    }

    public void Info(string message)
    {
        if (Level >= LogLevel.All)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;

            Console.WriteLine($"[INFO] {message}");

            Console.ResetColor();
        }
    }
}