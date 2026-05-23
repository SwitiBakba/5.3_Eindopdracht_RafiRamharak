using System.Security.Cryptography;

namespace PassTheWord.Replacements;

public class CharacterReplacementService
{
    public string Apply(string password, IReadOnlyDictionary<char, char> replacements)
    {
        if (replacements.Count == 0)
            return password;

        char[] chars = password.ToCharArray();

        for (int i = 0; i < chars.Length; i++)
        {
            if (replacements.TryGetValue(chars[i], out char replacement))
            {
                bool shouldReplace = RandomNumberGenerator.GetInt32(2) == 0;

                if (shouldReplace)
                    chars[i] = replacement;
            }
        }

        return new string(chars);
    }
}