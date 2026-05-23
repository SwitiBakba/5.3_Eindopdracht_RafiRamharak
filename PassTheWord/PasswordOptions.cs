using PassTheWord.Alphabets;

namespace PassTheWord;

public class PasswordOptions
{
    public int MinimumLength { get; set; } = 8;
    public int MaximumLength { get; set; } = 20;

    public bool UseUppercase { get; set; } = true;
    public bool UseLowercase { get; set; } = true;
    public bool UseDigits { get; set; } = true;
    public bool UseSymbols { get; set; } = true;

    public bool RequireUppercase { get; set; }
    public bool RequireDigit { get; set; }
    public bool RequireSymbol { get; set; }

    public bool ExcludeSimilarCharacters { get; set; }

    public List<IAlphabet> Alphabets { get; set; } = [new LatinAlphabet()];

    public Dictionary<char, char> Replacements { get; set; } = new();

    public List<string>? DictionaryWords { get; set; }

    public bool UseDictionary => DictionaryWords is { Count: > 0 };
}