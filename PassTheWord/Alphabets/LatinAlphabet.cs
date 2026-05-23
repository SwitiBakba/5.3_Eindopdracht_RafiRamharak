namespace PassTheWord.Alphabets;

/// <summary>
/// Provides Latin uppercase and lowercase characters.
/// </summary>
public class LatinAlphabet : IAlphabet
{
    public string Name => "Latin";

    public string UppercaseCharacters => "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    public string LowercaseCharacters => "abcdefghijklmnopqrstuvwxyz";
}