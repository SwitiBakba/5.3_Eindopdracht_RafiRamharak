using PassTheWord.Alphabets;
using PassTheWord.Logging;

namespace PassTheWord;

internal class Program
{
    static void Main(string[] args)
    {
        ILogger logger = new ConsoleLogger(LogLevel.All);

        PasswordService passwordService = new(logger);

        PasswordOptions options = new()
        {
            MinimumLength = 12,
            MaximumLength = 20,

            UseUppercase = true,
            UseLowercase = true,
            UseDigits = true,
            UseSymbols = true,

            RequireUppercase = true,
            RequireDigit = true,
            RequireSymbol = true,

            ExcludeSimilarCharacters = true,

            Alphabets = new List<IAlphabet>
            {
                new LatinAlphabet(),
                new CyrillicAlphabet()
            },

            Replacements = new Dictionary<char, char>
            {
                { 'o', '0' },
                { 'i', '1' },
                { 's', '$' }
            }
        };

        try
        {
            string password = passwordService.GeneratePassword(options);

            Console.WriteLine($"Generated password: {password}");
        }
        catch (Exception exception)
        {
            logger.Warning(exception.Message);
        }
    }
}