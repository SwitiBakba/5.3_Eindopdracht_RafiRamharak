using PassTheWord.Validation;

namespace PassTheWord.Tests.Validation;

[TestFixture]
public class PasswordOptionsValidatorTests
{
    private PasswordOptionsValidator _validator = null!;

    [SetUp]
    public void Setup()
    {
        _validator = new PasswordOptionsValidator();
    }

    [Test]
    public void Validate_WithValidOptions_DoesNotThrow()
    {
        var options = new PasswordOptions
        {
            MinimumLength = 8,
            MaximumLength = 20,
            UseUppercase = true,
            UseLowercase = true,
            UseDigits = true,
            UseSymbols = true
        };

        Assert.DoesNotThrow(() => _validator.Validate(options));
    }

    [Test]
    public void Validate_WithMinimumLengthZero_ThrowsArgumentException()
    {
        var options = new PasswordOptions
        {
            MinimumLength = 0
        };

        Assert.Throws<ArgumentException>(() => _validator.Validate(options));
    }

    [Test]
    public void Validate_WhenMaximumLengthIsSmallerThanMinimumLength_ThrowsArgumentException()
    {
        var options = new PasswordOptions
        {
            MinimumLength = 20,
            MaximumLength = 8
        };

        Assert.Throws<ArgumentException>(() => _validator.Validate(options));
    }

    [Test]
    public void Validate_WhenNoCharacterGroupsAreEnabled_ThrowsArgumentException()
    {
        var options = new PasswordOptions
        {
            UseUppercase = false,
            UseLowercase = false,
            UseDigits = false,
            UseSymbols = false
        };

        Assert.Throws<ArgumentException>(() => _validator.Validate(options));
    }

    [Test]
    public void Validate_WhenDigitIsRequiredButDigitsAreDisabled_ThrowsArgumentException()
    {
        var options = new PasswordOptions
        {
            UseDigits = false,
            RequireDigit = true
        };

        Assert.Throws<ArgumentException>(() => _validator.Validate(options));
    }

    [Test]
    public void Validate_WhenReplacementContainsSurrogate_ThrowsArgumentException()
    {
        var options = new PasswordOptions
        {
            Replacements = new Dictionary<char, char>
            {
                { 'a', '\uD800' }
            }
        };

        Assert.Throws<ArgumentException>(() => _validator.Validate(options));
    }
}