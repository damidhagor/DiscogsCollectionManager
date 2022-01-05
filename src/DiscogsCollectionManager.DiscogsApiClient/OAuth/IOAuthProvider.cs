namespace DiscogsCollectionManager.DiscogsApiClient.OAuth;

public delegate Task<string> GetVerifierCallback(string authorizeUrl, string verifierCallbackUrl, CancellationToken cancellationToken);

public interface IOAuthProvider
{
    bool IsAuthorized { get; }

    Task<(string accessToken, string accessTokenSecret)> AuthorizeAsync(string verifierCallbackUrl, GetVerifierCallback getVerifier, CancellationToken cancellationToken);

    HttpRequestMessage CreateAuthorizedRequest(HttpMethod httpMethod, string url);
}