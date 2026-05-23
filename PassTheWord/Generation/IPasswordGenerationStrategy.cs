namespace PassTheWord.Generation;

public interface IPasswordGenerationStrategy
{
    string Generate(PasswordOptions options);
}