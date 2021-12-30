using System.Text.Json;
using System.Net;
using DiscogsCollectionManager.DiscogsApiClient.Exceptions;
using DiscogsCollectionManager.DiscogsApiClient.OAuth;
using DiscogsCollectionManager.DiscogsApiClient.Contract;
using System.Globalization;
using DiscogsCollectionManager.DiscogsApiClient.Serialization;

namespace DiscogsCollectionManager.DiscogsApiClient;


public class DiscogsApiClient : IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly OAuthSession _oauthSession;

    private readonly string _userAgent;

    public bool IsAuthorized => _oauthSession.IsAuthorized;

    public DiscogsApiClient(string userAgent, string consumerKey, string consumerSecret, string accessToken = "", string accessTokenSecret = "")
    {
        _userAgent = userAgent;

        _oauthSession = new OAuthSession(userAgent, consumerKey, consumerSecret, accessToken, accessTokenSecret);

        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(_userAgent);
    }


    public async Task<(bool success, string accessToken, string accessTokenSecret)> AuthorizeAsync(string verifierCallbackUrl, GetVerifierCallback getVerifierCallback, CancellationToken cancellationToken)
    {
        var (accessToken, accessTokenSecret) = await _oauthSession.AuthorizeAsync(verifierCallbackUrl, getVerifierCallback, cancellationToken);
        var success = !String.IsNullOrWhiteSpace(accessToken) && !String.IsNullOrWhiteSpace(accessTokenSecret);

        return (success, accessToken, accessTokenSecret);
    }



    public async Task<Identity?> GetIdentityAsync(CancellationToken cancellationToken)
    {
        if (!IsAuthorized)
            throw new UnauthorizedDiscogsException();

        using var request = _oauthSession.CreateAuthorizedRequest(HttpMethod.Get, DiscogApiUrls.OAuthIdentityUrl);
        using var response = await _httpClient.SendAsync(request, cancellationToken);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
            throw new UnauthorizedDiscogsException();

        var content = await response.Content.ReadAsStringAsync(cancellationToken);

        var identity = await response.Content.DeserializeAsJson<Identity>();

        return identity;
    }

    public async Task<User?> GetUserAsync(string username, CancellationToken cancellationToken)
    {
        if (!IsAuthorized)
            throw new UnauthorizedDiscogsException();
        if (String.IsNullOrWhiteSpace(username))
            throw new ArgumentException(nameof(username));

        using var request = _oauthSession.CreateAuthorizedRequest(HttpMethod.Get, String.Format(DiscogApiUrls.UsersUrl, username));
        using var response = await _httpClient.SendAsync(request, cancellationToken);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
            throw new UnauthorizedDiscogsException();

        var user = await response.Content.DeserializeAsJson<User>();

        return user;
    }


    public void Dispose()
    {
        _httpClient?.Dispose();
    }
}
