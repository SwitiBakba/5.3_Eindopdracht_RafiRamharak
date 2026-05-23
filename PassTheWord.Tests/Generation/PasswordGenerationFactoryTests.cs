using PassTheWord.Generation;

namespace PassTheWord.Tests.Generation;

[TestFixture]
public class PasswordGenerationFactoryTests
{
    [Test]
    public void Create_WithoutDictionaryWords_ReturnsRandomPasswordStrategy()
    {
        var factory = new PasswordGenerationFactory();

        var options = new PasswordOptions
        {
            DictionaryWords = null
        };

        IPasswordGenerationStrategy strategy = factory.Create(options);

        Assert.That(strategy, Is.TypeOf<RandomPasswordStrategy>());
    }

    [Test]
    public void Create_WithDictionaryWords_ReturnsDictionaryPasswordStrategy()
    {
        var factory = new PasswordGenerationFactory();

        var options = new PasswordOptions
        {
            DictionaryWords = new List<string> { "hallo", "wereld" }
        };

        IPasswordGenerationStrategy strategy = factory.Create(options);

        Assert.That(strategy, Is.TypeOf<DictionaryPasswordStrategy>());
    }
}