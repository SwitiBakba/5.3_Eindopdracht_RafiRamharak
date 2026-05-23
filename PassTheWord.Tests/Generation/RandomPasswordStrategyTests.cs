using PassTheWord.Generation;

namespace PassTheWord.Tests.Generation;

[TestFixture]
public class RandomPasswordStrategyTests
{
    [Test]
    public void Generate_ReturnsPasswordWithinConfiguredLength()
    {
        var strategy = new RandomPasswordStrategy();

        var options = new PasswordOptions
        {
            MinimumLength = 12,
            MaximumLength = 20
        };

        string password = strategy.Generate(options);

        Assert.That(password.Length, Is.InRange(12, 20));
    }

    [Test]
    public void Generate_WhenDigitIsRequired_ContainsDigit()
    {
        var strategy = new RandomPasswordStrategy();

        var options = new PasswordOptions
        {
            MinimumLength = 12,
            MaximumLength = 12,
            UseDigits = true,
            RequireDigit = true
        };

        string password = strategy.Generate(options);

        Assert.That(password.Any(char.IsDigit), Is.True);
    }

    [Test]
    public void Generate_WhenSymbolIsRequired_ContainsSymbol()
    {
        var strategy = new RandomPasswordStrategy();

        var options = new PasswordOptions
        {
            MinimumLength = 12,
            MaximumLength = 12,
            UseSymbols = true,
            RequireSymbol = true
        };

        string password = strategy.Generate(options);

        Assert.That(password.Any(character => "!@#$%^&*()_+-=,./?~".Contains(character)), Is.True);
    }

    [Test]
    public void Generate_WhenSimilarCharactersAreExcluded_DoesNotContainSimilarCharacters()
    {
        var strategy = new RandomPasswordStrategy();

        var options = new PasswordOptions
        {
            MinimumLength = 50,
            MaximumLength = 50,
            UseUppercase = true,
            UseLowercase = true,
            UseDigits = true,
            UseSymbols = false,
            ExcludeSimilarCharacters = true
        };

        string password = strategy.Generate(options);

        Assert.That(password, Does.Not.Contain("I"));
        Assert.That(password, Does.Not.Contain("l"));
        Assert.That(password, Does.Not.Contain("1"));
        Assert.That(password, Does.Not.Contain("O"));
        Assert.That(password, Does.Not.Contain("0"));
    }
}