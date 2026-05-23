namespace PassTheWord.Validation;

public interface IExternalPasswordVerifier
{
    string Name { get; }

    string HashAlgorithmName { get; }

    bool IsSafe(string passwordHash);
}