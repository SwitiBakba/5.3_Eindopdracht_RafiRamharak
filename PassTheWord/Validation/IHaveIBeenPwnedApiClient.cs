namespace PassTheWord.Validation;

public interface IHaveIBeenPwnedApiClient
{
    string GetHashSuffixes(string hashPrefix);
}