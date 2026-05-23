using System.Security.Cryptography;

namespace PassTheWord.Generation;

public class DictionaryPasswordStrategy : IPasswordGenerationStrategy
{
    public string Generate(PasswordOptions options)
    {
        if (!options.UseDictionary)
            throw new InvalidOperationException("Dictionary strategy requires dictionary words.");

        List<string> words = options.DictionaryWords!;

        string password = "";

        while (password.Length < options.MinimumLength)
        {
            string word = words[RandomNumberGenerator.GetInt32(words.Count)];

            if (password.Length + word.Length > options.MaximumLength)
                continue;

            password += word;
        }

        return password;
    }
}