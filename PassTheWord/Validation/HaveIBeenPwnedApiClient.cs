namespace PassTheWord.Validation;

/// <summary>
/// Client for the Have I Been Pwned Pwned Passwords range API.
/// This class represents the external service/adaptee in the Adapter Pattern.
/// </summary>

public class HaveIBeenPwnedApiClient : IHaveIBeenPwnedApiClient
{
    private readonly HttpClient _httpClient;

    public HaveIBeenPwnedApiClient()
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://api.pwnedpasswords.com")
        };

        _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("PassTheWord");
    }

    public string GetHashSuffixes(string hashPrefix)
    {
        if (string.IsNullOrWhiteSpace(hashPrefix))
            throw new ArgumentException("Hash prefix cannot be empty.");

        if (hashPrefix.Length != 5)
            throw new ArgumentException("Hash prefix must be exactly 5 characters long.");

        HttpResponseMessage response = _httpClient
            .GetAsync($"/range/{hashPrefix}")
            .GetAwaiter()
            .GetResult();

        response.EnsureSuccessStatusCode();

        return response.Content
            .ReadAsStringAsync()
            .GetAwaiter()
            .GetResult();
    }
}