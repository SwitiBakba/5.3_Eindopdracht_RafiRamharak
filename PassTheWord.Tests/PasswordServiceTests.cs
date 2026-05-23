using PassTheWord.Logging;
using PassTheWord.Validation;

namespace PassTheWord.Tests;

[TestFixture]
public class PasswordServiceTests
{
    [Test]
    public void GeneratePassword_WithValidOptions_ReturnsPassword()
    {
        var logger = new ConsoleLogger(LogLevel.None);
        var service = new PasswordService(logger);

        var options = new PasswordOptions
        {
            MinimumLength = 12,
            MaximumLength = 20,
            UseUppercase = true,
            UseLowercase = true,
            UseDigits = true,
            UseSymbols = true,
            RequireUppercase = true,
            RequireDigit = true,
            RequireSymbol = true
        };

        string password = service.GeneratePassword(options);

        Assert.That(password, Is.Not.Null.And.Not.Empty);
        Assert.That(password.Length, Is.InRange(12, 20));
    }

    [Test]
    public void GeneratePassword_WithInvalidOptions_ThrowsArgumentException()
    {
        var logger = new ConsoleLogger(LogLevel.None);
        var service = new PasswordService(logger);

        var options = new PasswordOptions
        {
            MinimumLength = 20,
            MaximumLength = 8
        };

        Assert.Throws<ArgumentException>(() => service.GeneratePassword(options));
    }

    [Test]
    public void GeneratePassword_WhenExternalVerifierRejectsPassword_ThrowsInvalidOperationException()
    {
        var logger = new ConsoleLogger(LogLevel.None);
        var service = new PasswordService(logger);

        service.AddExternalVerifier(new RejectingExternalPasswordVerifier());

        var options = new PasswordOptions
        {
            MinimumLength = 8,
            MaximumLength = 12
        };

        Assert.Throws<InvalidOperationException>(() => service.GeneratePassword(options));
    }

    private class RejectingExternalPasswordVerifier : IExternalPasswordVerifier
    {
        public string Name => "Rejecting verifier";

        public string HashAlgorithmName => "SHA256";

        public bool IsSafe(string passwordHash)
        {
            return false;
        }
    }
}