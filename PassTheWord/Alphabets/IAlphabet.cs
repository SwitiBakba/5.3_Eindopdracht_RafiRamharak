namespace PassTheWord.Alphabets;

/// <summary>
/// Defines an alphabet that can be used for password generation.
/// </summary>
public interface IAlphabet
{
    string Name { get; }
    string UppercaseCharacters { get; }
    string LowercaseCharacters { get; }
}