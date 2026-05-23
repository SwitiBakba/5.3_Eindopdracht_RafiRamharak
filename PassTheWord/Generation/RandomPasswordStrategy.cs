using System.Security.Cryptography;

namespace PassTheWord.Generation;

public class RandomPasswordStrategy : IPasswordGenerationStrategy
{
    private const string Digits = "0123456789";
    private const string DigitsWithoutSimilar = "23456789";

    private const string Symbols = "!@#$%^&*()_+-=,./?~";

    private const string SimilarCharacters = "Il1O0";

    public string Generate(PasswordOptions options)
    {
        List<char> requiredCharacters = new();

        string uppercaseAlphabet = BuildUppercaseAlphabet(options);
        string lowercaseAlphabet = BuildLowercaseAlphabet(options);
        string digitAlphabet = options.ExcludeSimilarCharacters ? DigitsWithoutSimilar : Digits;
        string symbolAlphabet = Symbols;

        if (options.RequireUppercase)
            requiredCharacters.Add(GetRandomCharacter(uppercaseAlphabet));

        if (options.RequireDigit)
            requiredCharacters.Add(GetRandomCharacter(digitAlphabet));

        if (options.RequireSymbol)
            requiredCharacters.Add(GetRandomCharacter(symbolAlphabet));

        string fullAlphabet = BuildFullAlphabet(options);

        int length = RandomNumberGenerator.GetInt32(options.MinimumLength, options.MaximumLength + 1);

        List<char> passwordCharacters = [.. requiredCharacters];

        while (passwordCharacters.Count < length)
        {
            passwordCharacters.Add(GetRandomCharacter(fullAlphabet));
        }

        Shuffle(passwordCharacters);

        return new string([.. passwordCharacters]);
    }

    private static string BuildFullAlphabet(PasswordOptions options)
    {
        string alphabet = "";

        if (options.UseUppercase)
            alphabet += BuildUppercaseAlphabet(options);

        if (options.UseLowercase)
            alphabet += BuildLowercaseAlphabet(options);

        if (options.UseDigits)
            alphabet += options.ExcludeSimilarCharacters ? DigitsWithoutSimilar : Digits;

        if (options.UseSymbols)
            alphabet += Symbols;

        return alphabet;
    }

    private static string BuildUppercaseAlphabet(PasswordOptions options)
    {
        string characters = string.Concat(options.Alphabets.Select(a => a.UppercaseCharacters));

        return options.ExcludeSimilarCharacters
            ? RemoveSimilarCharacters(characters)
            : characters;
    }

    private static string BuildLowercaseAlphabet(PasswordOptions options)
    {
        string characters = string.Concat(options.Alphabets.Select(a => a.LowercaseCharacters));

        return options.ExcludeSimilarCharacters
            ? RemoveSimilarCharacters(characters)
            : characters;
    }

    private static string RemoveSimilarCharacters(string characters)
    {
        return new string(characters.Where(c => !SimilarCharacters.Contains(c)).ToArray());
    }

    private static char GetRandomCharacter(string characters)
    {
        int index = RandomNumberGenerator.GetInt32(characters.Length);
        return characters[index];
    }

    private static void Shuffle(List<char> characters)
    {
        for (int i = characters.Count - 1; i > 0; i--)
        {
            int j = RandomNumberGenerator.GetInt32(i + 1);

            (characters[i], characters[j]) = (characters[j], characters[i]);
        }
    }
}