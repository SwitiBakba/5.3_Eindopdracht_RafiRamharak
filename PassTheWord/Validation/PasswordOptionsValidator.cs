namespace PassTheWord.Validation;

/// <summary>
/// Validates password options before password generation starts.
/// </summary>
public class PasswordOptionsValidator
{
    public void Validate(PasswordOptions options)
    {
        if (options.MinimumLength <= 0)
            throw new ArgumentException("Minimum length must be greater than 0.");

        if (options.MaximumLength < options.MinimumLength)
            throw new ArgumentException("Maximum length must be greater than or equal to minimum length.");

        if (!options.UseUppercase && !options.UseLowercase && !options.UseDigits && !options.UseSymbols)
            throw new ArgumentException("At least one character group must be enabled.");

        if (options.RequireUppercase && !options.UseUppercase)
            throw new ArgumentException("Uppercase cannot be required when uppercase characters are disabled.");

        if (options.RequireDigit && !options.UseDigits)
            throw new ArgumentException("Digits cannot be required when digits are disabled.");

        if (options.RequireSymbol && !options.UseSymbols)
            throw new ArgumentException("Symbols cannot be required when symbols are disabled.");

        if (options.Alphabets.Count == 0 && (options.UseUppercase || options.UseLowercase))
            throw new ArgumentException("At least one alphabet is required when letters are enabled.");

        foreach (var alphabet in options.Alphabets)
        {
            // Surrogate characters are rejected because this assignment focuses on characters in the Basic Multilingual Plane.
            ValidateNoSurrogates(alphabet.UppercaseCharacters, $"{alphabet.Name} uppercase characters");
            ValidateNoSurrogates(alphabet.LowercaseCharacters, $"{alphabet.Name} lowercase characters");
        }

        foreach (var replacement in options.Replacements)
        {
            if (char.IsSurrogate(replacement.Key) || char.IsSurrogate(replacement.Value))
                throw new ArgumentException("Replacements cannot contain surrogate characters.");
        }

        if (options.UseDictionary)
        {
            foreach (string word in options.DictionaryWords!)
            {
                ValidateNoSurrogates(word, $"Dictionary word '{word}'");
            }
        }
    }

    private static void ValidateNoSurrogates(string value, string description)
    {
        foreach (char character in value)
        {
            if (char.IsSurrogate(character))
                throw new ArgumentException($"{description} contains a surrogate character.");
        }
    }
}