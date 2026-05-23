namespace PassTheWord.Alphabets;

public class CyrillicAlphabet : IAlphabet
{
    public string Name => "Cyrillic";

    public string UppercaseCharacters => "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";

    public string LowercaseCharacters => "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";
}