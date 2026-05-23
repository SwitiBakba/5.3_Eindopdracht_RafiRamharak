using System.Security.Cryptography;
using System.Text;
using PassTheWord.Logging;

namespace PassTheWord.Validation;

/// <summary>
/// Handles runtime-registered external password verifiers.
/// </summary>
public class ExternalPasswordValidationService
{
    private readonly List<IExternalPasswordVerifier> _verifiers = [];
    private readonly ILogger _logger;

    public ExternalPasswordValidationService(ILogger logger)
    {
        _logger = logger;
    }

    public void AddVerifier(IExternalPasswordVerifier verifier)
    {
        _verifiers.Add(verifier);
        _logger.Info($"External password verifier added: {verifier.Name}");
    }

    public bool IsSafe(string password)
    {
        if (_verifiers.Count == 0)
        {
            _logger.Info("No external password verifiers configured.");
            return true;
        }

        foreach (IExternalPasswordVerifier verifier in _verifiers)
        {
            _logger.Info($"Checking password with external verifier: {verifier.Name}.");
            string hash = HashPassword(password, verifier.HashAlgorithmName);

            bool isSafe = verifier.IsSafe(hash);

            if (!isSafe)
            {
                _logger.Warning($"Password was rejected by external verifier: {verifier.Name}");
                return false;
            }
        }

        return true;
    }

    private static string HashPassword(string password, string hashAlgorithmName)
    {
        byte[] input = Encoding.UTF8.GetBytes(password);

        byte[] hash = hashAlgorithmName.ToUpperInvariant() switch
        {
            "SHA256" => SHA256.HashData(input),
            "SHA1" => SHA1.HashData(input),
            _ => throw new NotSupportedException($"Hash algorithm '{hashAlgorithmName}' is not supported.")
        };

        return Convert.ToHexString(hash);
    }
}