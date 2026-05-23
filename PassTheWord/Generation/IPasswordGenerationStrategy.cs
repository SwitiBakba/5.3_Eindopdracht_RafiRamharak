namespace PassTheWord.Generation;

/// <summary>
/// Strategy interface for different password generation algorithms.
/// </summary>
public interface IPasswordGenerationStrategy
{
    string Generate(PasswordOptions options);
}