namespace PassTheWord.Generation;

/// <summary>
/// Factory responsible for selecting the correct password generation strategy.
/// </summary>
public class PasswordGenerationFactory
{
    public IPasswordGenerationStrategy Create(PasswordOptions options)
    {
        if (options.UseDictionary)
            return new DictionaryPasswordStrategy();

        return new RandomPasswordStrategy();
    }
}