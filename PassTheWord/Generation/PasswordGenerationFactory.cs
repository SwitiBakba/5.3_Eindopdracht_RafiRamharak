namespace PassTheWord.Generation;

public class PasswordGenerationFactory
{
    public IPasswordGenerationStrategy Create(PasswordOptions options)
    {
        if (options.UseDictionary)
            return new DictionaryPasswordStrategy();

        return new RandomPasswordStrategy();
    }
}