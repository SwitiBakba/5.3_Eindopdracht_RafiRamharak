using PassTheWord.Generation;
using PassTheWord.Logging;
using PassTheWord.Replacements;
using PassTheWord.Validation;

namespace PassTheWord;

/// <summary>
/// Facade that coordinates validation, password generation, replacements and external verification.
/// </summary>
public class PasswordService
{
    private readonly PasswordOptionsValidator _optionsValidator;
    private readonly PasswordGenerationFactory _generationFactory;
    private readonly CharacterReplacementService _replacementService;
    private readonly ExternalPasswordValidationService _externalValidationService;
    private readonly ILogger _logger;

    public PasswordService(ILogger logger)
    {
        _logger = logger;
        _optionsValidator = new PasswordOptionsValidator();
        _generationFactory = new PasswordGenerationFactory();
        _replacementService = new CharacterReplacementService();
        _externalValidationService = new ExternalPasswordValidationService(logger);
    }

    public void AddExternalVerifier(IExternalPasswordVerifier verifier)
    {
        _externalValidationService.AddVerifier(verifier);
    }

    public string GeneratePassword(PasswordOptions options)
    {
        _logger.Info("Validating password options.");
        _optionsValidator.Validate(options);

        IPasswordGenerationStrategy strategy = _generationFactory.Create(options);
        _logger.Info($"Selected generation strategy: {strategy.GetType().Name}.");

        for (int attempt = 1; attempt <= 10; attempt++)
        {
            _logger.Info($"Generating password. Attempt {attempt}.");

            string password = strategy.Generate(options);

            password = _replacementService.Apply(password, options.Replacements);

            if (_externalValidationService.IsSafe(password))
            {
                _logger.Info("Password generated successfully.");
                return password;
            }
        }

        _logger.Warning("Password generation failed after all attempts.");
        throw new InvalidOperationException("Could not generate a safe password after 10 attempts.");
    }
}