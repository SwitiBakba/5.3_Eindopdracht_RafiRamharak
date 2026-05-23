using PassTheWord.Logging;

namespace PassTheWord.Tests.Logging;

[TestFixture]
public class ConsoleLoggerTests
{
    private TextWriter _originalOutput = null!;

    [SetUp]
    public void Setup()
    {
        _originalOutput = Console.Out;
    }

    [TearDown]
    public void Cleanup()
    {
        Console.SetOut(_originalOutput);
        Console.ResetColor();
    }

    [Test]
    public void Info_WithLogLevelAll_WritesInfoMessage()
    {
        // Arrange
        var logger = new ConsoleLogger(LogLevel.All);

        using var output = new StringWriter();
        Console.SetOut(output);

        // Act
        logger.Info("Test message");

        // Assert
        Assert.That(output.ToString(),
            Does.Contain("[INFO] Test message"));
    }

    [Test]
    public void Info_WithLogLevelWarnings_DoesNotWriteInfoMessage()
    {
        // Arrange
        var logger = new ConsoleLogger(LogLevel.Warnings);

        using var output = new StringWriter();
        Console.SetOut(output);

        // Act
        logger.Info("Test message");

        // Assert
        Assert.That(output.ToString(), Is.Empty);
    }

    [Test]
    public void Warning_WithLogLevelWarnings_WritesWarningMessage()
    {
        // Arrange
        var logger = new ConsoleLogger(LogLevel.Warnings);

        using var output = new StringWriter();
        Console.SetOut(output);

        // Act
        logger.Warning("Something went wrong");

        // Assert
        Assert.That(output.ToString(),
            Does.Contain("[WARNING] Something went wrong"));
    }

    [Test]
    public void Warning_WithLogLevelNone_DoesNotWriteWarningMessage()
    {
        // Arrange
        var logger = new ConsoleLogger(LogLevel.None);

        using var output = new StringWriter();
        Console.SetOut(output);

        // Act
        logger.Warning("Something went wrong");

        // Assert
        Assert.That(output.ToString(), Is.Empty);
    }
}