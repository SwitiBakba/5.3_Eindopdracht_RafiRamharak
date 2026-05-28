using PassTheWord.Alphabets;
using PassTheWord.Logging;
using PassTheWord.Validation;

namespace PassTheWord;

internal class Program
{
    static void Main(string[] args)
    {
        ILogger logger = new ConsoleLogger(LogLevel.All);

        logger.Info("PassTheWord started.");

        PasswordService passwordService = new(logger);

        HaveIBeenPwnedApiClient haveIBeenPwnedApiClient = new();
        HaveIBeenPwnedPasswordVerifierAdapter haveIBeenPwnedAdapter = new(haveIBeenPwnedApiClient);

        bool useHaveIBeenPwned = true;

        if (useHaveIBeenPwned)
        {
            passwordService.AddExternalVerifier(haveIBeenPwnedAdapter);
        }

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

            Alphabets =
            [
                new LatinAlphabet(),
                new CyrillicAlphabet()
            ],

            Replacements = new Dictionary<char, char>
            {
                { 'o', '0' },
                { 'i', '1' },
                { 's', '$' }
            },

            // Optional for DictionaryPasswordStrategy
            //DictionaryWords =
            //[
            //    "hallo",
            //    "wereld",
            //    "kat",
            //    "hond"
            //]

        };

        logger.Info("Password options configured.");

        try
        {
            string password = passwordService.GeneratePassword(options);

            Console.WriteLine($"Generated password: {password}");
        }
        catch (Exception exception)
        {
            logger.Warning(exception.Message);
        }

        logger.Info("PassTheWord finished.");
    }
}
