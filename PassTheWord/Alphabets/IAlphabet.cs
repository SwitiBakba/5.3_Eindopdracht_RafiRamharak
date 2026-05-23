namespace PassTheWord.Alphabets;

public interface IAlphabet
{
    string Name { get; }
    string UppercaseCharacters { get; }
    string LowercaseCharacters { get; }
}