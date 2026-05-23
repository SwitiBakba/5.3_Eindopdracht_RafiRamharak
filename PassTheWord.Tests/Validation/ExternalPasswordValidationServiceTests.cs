using PassTheWord.Logging;
using PassTheWord.Validation;

namespace PassTheWord.Tests.Validation;

[TestFixture]
public class ExternalPasswordValidationServiceTests
{
    [Test]
    public void IsSafe_WithoutVerifiers_ReturnsTrue()
    {
        var logger = new ConsoleLogger(LogLevel.None);
        var service = new ExternalPasswordValidationService(logger);

        bool result = service.IsSafe("password");

        Assert.That(result, Is.True);
    }

    [Test]
    public void IsSafe_WhenVerifierAcceptsPassword_ReturnsTrue()
    {
        var logger = new ConsoleLogger(LogLevel.None);
        var service = new ExternalPasswordValidationService(logger);

        service.AddVerifier(new FakeExternalPasswordVerifier(true));

        bool result = service.IsSafe("password");

        Assert.That(result, Is.True);
    }

    [Test]
    public void IsSafe_WhenVerifierRejectsPassword_ReturnsFalse()
    {
        var logger = new ConsoleLogger(LogLevel.None);
        var service = new ExternalPasswordValidationService(logger);

        service.AddVerifier(new FakeExternalPasswordVerifier(false));

        bool result = service.IsSafe("password");

        Assert.That(result, Is.False);
    }

    [Test]
    public void IsSafe_PassesHashToVerifierInsteadOfPlainPassword()
    {
        var logger = new ConsoleLogger(LogLevel.None);
        var verifier = new CapturingExternalPasswordVerifier();

        var service = new ExternalPasswordValidationService(logger);
        service.AddVerifier(verifier);

        service.IsSafe("password");

        Assert.That(verifier.ReceivedValue, Is.Not.EqualTo("password"));
        Assert.That(verifier.ReceivedValue, Is.Not.Null);
        Assert.That(verifier.ReceivedValue!.Length, Is.GreaterThan(0));
    }

    private class FakeExternalPasswordVerifier : IExternalPasswordVerifier
    {
        private readonly bool _isSafe;

        public FakeExternalPasswordVerifier(bool isSafe)
        {
            _isSafe = isSafe;
        }

        public string Name => "Fake verifier";

        public string HashAlgorithmName => "SHA256";

        public bool IsSafe(string passwordHash)
        {
            return _isSafe;
        }
    }

    private class CapturingExternalPasswordVerifier : IExternalPasswordVerifier
    {
        public string? ReceivedValue { get; private set; }

        public string Name => "Capturing verifier";

        public string HashAlgorithmName => "SHA256";

        public bool IsSafe(string passwordHash)
        {
            ReceivedValue = passwordHash;
            return true;
        }
    }
}