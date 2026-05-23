namespace PassTheWord.Alphabets;

public class LatinAlphabet : IAlphabet
{
    public string Name => "Latin";

    public string UppercaseCharacters => "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    public string LowercaseCharacters => "abcdefghijklmnopqrstuvwxyz";
}