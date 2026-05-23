namespace PassTheWord.Alphabets;

/// <summary>
/// Provides Russian Cyrillic uppercase and lowercase characters as demonstration of alphabet extensibility.
/// </summary>
public class CyrillicAlphabet : IAlphabet
{
    public string Name => "Cyrillic";

    public string UppercaseCharacters => "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";

    public string LowercaseCharacters => "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";
}