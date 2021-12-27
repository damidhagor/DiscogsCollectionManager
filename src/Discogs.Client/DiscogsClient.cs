using System.Text.Json;
using System.Net;
using Discogs.Client.Exceptions;
using Discogs.Client.OAuth;
using Discogs.Client.Contract;

namespace Discogs.Client;


public class DiscogsClient : IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly OAuthSession _oauthSession;

    private readonly string _userAgent;

    public bool IsAuthorized => _oauthSession.IsAuthorized;

    public DiscogsClient(string userAgent, string consumerKey, string consumerSecret, string accessToken = "", string accessTokenSecret = "")
    {
        _userAgent = userAgent;

        _oauthSession = new OAuthSession(userAgent, consumerKey, consumerSecret, accessToken, accessTokenSecret);

        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(_userAgent);
    }


    public async Task<bool> AuthorizeAsync(string verifierCallbackUrl, GetVerifierCallback getVerifierCallback, CancellationToken cancellationToken)
    {
        bool result = await _oauthSession.AuthorizeAsync(verifierCallbackUrl, getVerifierCallback, cancellationToken);

        return result;
    }



    public async Task<Identity?> GetIdentity(CancellationToken cancellationToken)
    {
        if (!IsAuthorized)
            throw new UnauthorizedDiscogsException();

        using var request = _oauthSession.CreateAuthorizedRequest(HttpMethod.Get, DiscogApiUrls.OAuthIdentityUrl);
        using var response = await _httpClient.SendAsync(request, cancellationToken);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
            throw new UnauthorizedDiscogsException();

        var content = await response.Content.ReadAsStringAsync(cancellationToken);

        var identity = JsonSerializer.Deserialize<Identity>(content);

        return identity;
    }

    public async Task<User?> GetUser(string username, CancellationToken cancellationToken)
    {
        if (!IsAuthorized)
            throw new UnauthorizedDiscogsException();
        if (String.IsNullOrWhiteSpace(username))
            throw new ArgumentException(nameof(username));

        using var request = _oauthSession.CreateAuthorizedRequest(HttpMethod.Get, String.Format(DiscogApiUrls.UsersUrl, username));
        using var response = await _httpClient.SendAsync(request, cancellationToken);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
            throw new UnauthorizedDiscogsException();

        var content = await response.Content.ReadAsStringAsync(cancellationToken);

        var user = JsonSerializer.Deserialize<User>(content);

        return user;
    }


    public void Dispose()
    {
        _httpClient?.Dispose();
    }
}
