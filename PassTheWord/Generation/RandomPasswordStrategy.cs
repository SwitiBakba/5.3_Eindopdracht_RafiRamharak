using System.Security.Cryptography;

namespace PassTheWord.Generation;

/// <summary>
/// Generates passwords by randomly selecting characters from the configured alphabets and character groups.
/// </summary>
public class RandomPasswordStrategy : IPasswordGenerationStrategy
{
    private const string Digits = "0123456789";
    private const string DigitsWithoutSimilar = "23456789";
    private const string Symbols = "!@#$%^&*()_+-=,./?~";
    private const string SimilarCharacters = "Il1O0";

    public string Generate(PasswordOptions options)
    {
        List<char> requiredCharacters = new();

        string uppercaseAlphabet = options.UseUppercase
            ? BuildUppercaseAlphabet(options)
            : "";

        string lowercaseAlphabet = options.UseLowercase
            ? BuildLowercaseAlphabet(options)
            : "";

        string digitAlphabet = options.UseDigits
            ? options.ExcludeSimilarCharacters ? DigitsWithoutSimilar : Digits
            : "";

        string symbolAlphabet = options.UseSymbols
            ? Symbols
            : "";

        if (options.RequireUppercase)
            requiredCharacters.Add(GetRandomCharacter(uppercaseAlphabet));

        if (options.RequireDigit)
            requiredCharacters.Add(GetRandomCharacter(digitAlphabet));

        if (options.RequireSymbol)
            requiredCharacters.Add(GetRandomCharacter(symbolAlphabet));

        // Shuffle required characters so they do not always appear at the beginning.

        string fullAlphabet = uppercaseAlphabet + lowercaseAlphabet + digitAlphabet + symbolAlphabet;

        int length = RandomNumberGenerator.GetInt32(
            options.MinimumLength,
            options.MaximumLength + 1
        );

        List<char> passwordCharacters = [.. requiredCharacters];

        while (passwordCharacters.Count < length)
        {
            passwordCharacters.Add(GetRandomCharacter(fullAlphabet));
        }

        Shuffle(passwordCharacters);

        return new string([.. passwordCharacters]);
    }

    private static string BuildUppercaseAlphabet(PasswordOptions options)
    {
        string characters = string.Concat(
            options.Alphabets.Select(alphabet => alphabet.UppercaseCharacters)
        );

        return options.ExcludeSimilarCharacters
            ? RemoveSimilarCharacters(characters)
            : characters;
    }

    private static string BuildLowercaseAlphabet(PasswordOptions options)
    {
        string characters = string.Concat(
            options.Alphabets.Select(alphabet => alphabet.LowercaseCharacters)
        );

        return options.ExcludeSimilarCharacters
            ? RemoveSimilarCharacters(characters)
            : characters;
    }

    private static string RemoveSimilarCharacters(string characters)
    {
        return new string(
            characters
                .Where(character => !SimilarCharacters.Contains(character))
                .ToArray()
        );
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
            int randomIndex = RandomNumberGenerator.GetInt32(i + 1);

            (characters[i], characters[randomIndex]) =
                (characters[randomIndex], characters[i]);
        }
    }
}