namespace PassTheWord.Validation;

/// <summary>
/// Adapter interface for external password verification services.
/// Implementations receive only a password hash, never the plain password.
/// </summary>
public interface IExternalPasswordVerifier
{
    string Name { get; }

    string HashAlgorithmName { get; }

    bool IsSafe(string passwordHash);
}