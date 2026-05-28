namespace PassTheWord.Validation;

/// <summary>
/// Adapter that makes the Have I Been Pwned API usable through the application's
/// IExternalPasswordVerifier interface.
/// </summary>
public class HaveIBeenPwnedPasswordVerifierAdapter : IExternalPasswordVerifier
{
    private readonly IHaveIBeenPwnedApiClient _apiClient;

    public HaveIBeenPwnedPasswordVerifierAdapter(IHaveIBeenPwnedApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public string Name => "Have I Been Pwned";

    public string HashAlgorithmName => "SHA1";

    public bool IsSafe(string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("Password hash cannot be empty.");

        if (passwordHash.Length != 40)
            throw new ArgumentException("SHA1 password hash must be 40 characters long.");

        string normalizedHash = passwordHash.ToUpperInvariant();

        string prefix = normalizedHash[..5];
        string suffix = normalizedHash[5..];

        string response = _apiClient.GetHashSuffixes(prefix);

        string[] lines = response.Split(
            new[] { "\r\n", "\n" },
            StringSplitOptions.RemoveEmptyEntries
        );

        foreach (string line in lines)
        {
            string leakedHashSuffix = line.Split(':')[0];

            if (leakedHashSuffix.Equals(suffix, StringComparison.OrdinalIgnoreCase))
                return false;
        }

        return true;
    }
}