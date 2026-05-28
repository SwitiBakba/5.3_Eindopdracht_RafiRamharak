using PassTheWord.Validation;

namespace PassTheWord.Tests.Validation;

[TestFixture]
public class HaveIBeenPwnedPasswordVerifierAdapterTests
{
    [Test]
    public void IsSafe_WhenHashSuffixExistsInResponse_ReturnsFalse()
    {
        var apiClient = new FakeHaveIBeenPwnedApiClient(
            "67890123456789012345678901234567890:5"
        );

        var adapter = new HaveIBeenPwnedPasswordVerifierAdapter(apiClient);

        bool result = adapter.IsSafe("ABCDE67890123456789012345678901234567890");

        Assert.That(result, Is.False);
    }

    [Test]
    public void IsSafe_WhenHashSuffixDoesNotExistInResponse_ReturnsTrue()
    {
        var apiClient = new FakeHaveIBeenPwnedApiClient(
            "11111111111111111111111111111111111:5"
        );

        var adapter = new HaveIBeenPwnedPasswordVerifierAdapter(apiClient);

        bool result = adapter.IsSafe("ABCDE67890123456789012345678901234567890");

        Assert.That(result, Is.True);
    }

    [Test]
    public void IsSafe_UsesFirstFiveCharactersAsPrefix()
    {
        var apiClient = new FakeHaveIBeenPwnedApiClient("");

        var adapter = new HaveIBeenPwnedPasswordVerifierAdapter(apiClient);

        adapter.IsSafe("ABCDE67890123456789012345678901234567890");

        Assert.That(apiClient.ReceivedPrefix, Is.EqualTo("ABCDE"));
    }

    [Test]
    public void IsSafe_WithEmptyHash_ThrowsArgumentException()
    {
        var apiClient = new FakeHaveIBeenPwnedApiClient("");
        var adapter = new HaveIBeenPwnedPasswordVerifierAdapter(apiClient);

        Assert.Throws<ArgumentException>(() => adapter.IsSafe(""));
    }

    [Test]
    public void IsSafe_WithInvalidSha1Length_ThrowsArgumentException()
    {
        var apiClient = new FakeHaveIBeenPwnedApiClient("");
        var adapter = new HaveIBeenPwnedPasswordVerifierAdapter(apiClient);

        Assert.Throws<ArgumentException>(() => adapter.IsSafe("ABC"));
    }

    private class FakeHaveIBeenPwnedApiClient : IHaveIBeenPwnedApiClient
    {
        private readonly string _response;

        public string? ReceivedPrefix { get; private set; }

        public FakeHaveIBeenPwnedApiClient(string response)
        {
            _response = response;
        }

        public string GetHashSuffixes(string hashPrefix)
        {
            ReceivedPrefix = hashPrefix;
            return _response;
        }
    }
}