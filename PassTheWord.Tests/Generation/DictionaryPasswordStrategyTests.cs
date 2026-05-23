using PassTheWord.Generation;

namespace PassTheWord.Tests.Generation;

[TestFixture]
public class DictionaryPasswordStrategyTests
{
    [Test]
    public void Generate_WithDictionaryWords_ReturnsPasswordWithinLengthLimits()
    {
        var strategy = new DictionaryPasswordStrategy();

        var options = new PasswordOptions
        {
            MinimumLength = 8,
            MaximumLength = 20,
            DictionaryWords = new List<string>
            {
                "hallo",
                "kat",
                "hond",
                "paard"
            }
        };

        string password = strategy.Generate(options);

        Assert.That(password.Length, Is.InRange(8, 20));
    }

    [Test]
    public void Generate_WithoutDictionaryWords_ThrowsInvalidOperationException()
    {
        var strategy = new DictionaryPasswordStrategy();

        var options = new PasswordOptions
        {
            DictionaryWords = null
        };

        Assert.Throws<InvalidOperationException>(() => strategy.Generate(options));
    }

    [Test]
    public void Generate_WhenWordsCannotFitWithinMaximumLength_ThrowsInvalidOperationException()
    {
        var strategy = new DictionaryPasswordStrategy();

        var options = new PasswordOptions
        {
            MinimumLength = 8,
            MaximumLength = 8,
            DictionaryWords = new List<string>
            {
                "abcde"
            }
        };

        Assert.Throws<InvalidOperationException>(() => strategy.Generate(options));
    }
}