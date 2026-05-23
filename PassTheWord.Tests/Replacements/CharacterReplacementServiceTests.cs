using PassTheWord.Replacements;

namespace PassTheWord.Tests.Replacements;

[TestFixture]
public class CharacterReplacementServiceTests
{
    [Test]
    public void Apply_WithNoReplacements_ReturnsOriginalPassword()
    {
        var service = new CharacterReplacementService();

        string result = service.Apply("password", new Dictionary<char, char>());

        Assert.That(result, Is.EqualTo("password"));
    }

    [Test]
    public void Apply_WithReplacements_KeepsPasswordLength()
    {
        var service = new CharacterReplacementService();

        string result = service.Apply(
            "oooo",
            new Dictionary<char, char>
            {
                { 'o', '0' }
            }
        );

        Assert.That(result.Length, Is.EqualTo(4));
    }

    [Test]
    public void Apply_WithReplacements_OnlyUsesOriginalOrReplacementCharacters()
    {
        var service = new CharacterReplacementService();

        string result = service.Apply(
            "oooo",
            new Dictionary<char, char>
            {
                { 'o', '0' }
            }
        );

        Assert.That(result.All(character => character is 'o' or '0'), Is.True);
    }
}