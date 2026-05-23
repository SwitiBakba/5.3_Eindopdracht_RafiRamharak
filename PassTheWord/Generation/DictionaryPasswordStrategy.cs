using System.Security.Cryptography;

namespace PassTheWord.Generation;

public class DictionaryPasswordStrategy : IPasswordGenerationStrategy
{
    private const int MaximumAttempts = 1000;

    public string Generate(PasswordOptions options)
    {
        if (!options.UseDictionary)
            throw new InvalidOperationException("Dictionary strategy requires dictionary words.");

        List<string> words = options.DictionaryWords!;

        string password = "";
        int attempts = 0;

        while (password.Length < options.MinimumLength)
        {
            attempts++;

            if (attempts > MaximumAttempts)
            {
                throw new InvalidOperationException(
                    "Could not generate a dictionary password within the configured length limits."
                );
            }

            string word = words[RandomNumberGenerator.GetInt32(words.Count)];

            if (password.Length + word.Length > options.MaximumLength)
                continue;

            password += word;
        }

        return password;
    }
}