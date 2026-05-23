using PassTheWord.Alphabets;

namespace PassTheWord.Tests.Alphabets;

[TestFixture]
public class AlphabetTests
{
    [Test]
    public void LatinAlphabet_ContainsLatinCharacters()
    {
        var alphabet = new LatinAlphabet();

        Assert.That(alphabet.Name, Is.EqualTo("Latin"));
        Assert.That(alphabet.UppercaseCharacters, Does.Contain("A"));
        Assert.That(alphabet.LowercaseCharacters, Does.Contain("z"));
    }

    [Test]
    public void CyrillicAlphabet_ContainsCyrillicCharacters()
    {
        var alphabet = new CyrillicAlphabet();

        Assert.That(alphabet.Name, Is.EqualTo("Cyrillic"));
        Assert.That(alphabet.UppercaseCharacters, Does.Contain("А"));
        Assert.That(alphabet.LowercaseCharacters, Does.Contain("я"));
    }
}