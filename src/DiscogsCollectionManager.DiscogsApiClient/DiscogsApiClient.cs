using System.Net;
using DiscogsCollectionManager.DiscogsApiClient.Contract;
using DiscogsCollectionManager.DiscogsApiClient.Exceptions;
using DiscogsCollectionManager.DiscogsApiClient.OAuth;
using DiscogsCollectionManager.DiscogsApiClient.Serialization;

namespace DiscogsCollectionManager.DiscogsApiClient;


public class DiscogsApiClient : IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly IOAuthProvider _oAuthProvider;

    private readonly string _userAgent;

    public bool IsAuthorized => _oAuthProvider.IsAuthorized;

    public DiscogsApiClient(IOAuthProvider oAuthProvider, string userAgent)
    {
        _userAgent = userAgent;

        _oAuthProvider = oAuthProvider;

        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(_userAgent);
    }


    public async Task<(bool success, string accessToken, string accessTokenSecret)> AuthorizeAsync(string verifierCallbackUrl, GetVerifierCallback getVerifierCallback, CancellationToken cancellationToken)
    {
        var (accessToken, accessTokenSecret) = await _oAuthProvider.AuthorizeAsync(verifierCallbackUrl, getVerifierCallback, cancellationToken);
        var success = !String.IsNullOrWhiteSpace(accessToken) && !String.IsNullOrWhiteSpace(accessTokenSecret);

        return (success, accessToken, accessTokenSecret);
    }



    public async Task<Identity> GetIdentityAsync(CancellationToken cancellationToken)
    {
        if (!IsAuthorized)
            throw new UnauthorizedDiscogsException();

        using var request = _oAuthProvider.CreateAuthorizedRequest(HttpMethod.Get, DiscogApiUrls.OAuthIdentityUrl);
        using var response = await _httpClient.SendAsync(request, cancellationToken);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
            throw new UnauthorizedDiscogsException();

        var content = await response.Content.ReadAsStringAsync(cancellationToken);

        var identity = await response.Content.DeserializeAsJsonAsync<Identity>(cancellationToken);

        return identity;
    }

    public async Task<User> GetUserAsync(string username, CancellationToken cancellationToken)
    {
        if (!IsAuthorized)
            throw new UnauthorizedDiscogsException();
        if (String.IsNullOrWhiteSpace(username))
            throw new ArgumentException(nameof(username));

        using var request = _oAuthProvider.CreateAuthorizedRequest(HttpMethod.Get, String.Format(DiscogApiUrls.UsersUrl, username));
        using var response = await _httpClient.SendAsync(request, cancellationToken);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
            throw new UnauthorizedDiscogsException();

        var user = await response.Content.DeserializeAsJsonAsync<User>(cancellationToken);

        return user;
    }


    #region Collection
    public async Task<List<CollectionFolder>> GetCollectionFoldersAsync(string username, CancellationToken cancellationToken)
    {
        if (!IsAuthorized)
            throw new UnauthorizedDiscogsException();
        if (String.IsNullOrWhiteSpace(username))
            throw new ArgumentException(nameof(username));

        using var request = _oAuthProvider.CreateAuthorizedRequest(HttpMethod.Get, String.Format(DiscogApiUrls.CollectionFoldersUrl, username));
        using var response = await _httpClient.SendAsync(request, cancellationToken);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
            throw new UnauthorizedDiscogsException();

        var collectionFoldersResponse = await response.Content.DeserializeAsJsonAsync<CollectionFoldersResponse>(cancellationToken);

        return collectionFoldersResponse.Folders;
    }

    public async Task<CollectionFolder?> GetCollectionFolderAsync(string username, int folderId, CancellationToken cancellationToken)
    {
        if (!IsAuthorized)
            throw new UnauthorizedDiscogsException();
        if (String.IsNullOrWhiteSpace(username))
            throw new ArgumentException(nameof(username));

        using var request = _oAuthProvider.CreateAuthorizedRequest(HttpMethod.Get, $"{String.Format(DiscogApiUrls.CollectionFoldersUrl, username)}/{folderId}");
        using var response = await _httpClient.SendAsync(request, cancellationToken);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
            throw new UnauthorizedDiscogsException();

        var collectionFolder = response.StatusCode == HttpStatusCode.OK ? await response.Content.DeserializeAsJsonAsync<CollectionFolder>(cancellationToken) : null;

        return collectionFolder;
    }

    public async Task<CollectionFolder?> CreateCollectionFolderAsync(string username, CreateCollectionFolderRequest createFolderRequest, CancellationToken cancellationToken)
    {
        if (!IsAuthorized)
            throw new UnauthorizedDiscogsException();
        if (String.IsNullOrWhiteSpace(username))
            throw new ArgumentException(nameof(username));
        if (String.IsNullOrWhiteSpace(createFolderRequest.Name))
            throw new ArgumentException(nameof(createFolderRequest));

        using var request = _oAuthProvider.CreateAuthorizedRequest(HttpMethod.Post, String.Format(DiscogApiUrls.CollectionFoldersUrl, username));

        request.Content = CreateJsonContent(createFolderRequest);

        using var response = await _httpClient.SendAsync(request, cancellationToken);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
            throw new UnauthorizedDiscogsException();

        var collectionFolder = await response.Content.DeserializeAsJsonAsync<CollectionFolder>(cancellationToken);

        return collectionFolder;
    }

    public async Task<CollectionFolder?> UpdateCollectionFolderAsync(string username, int folderId, CreateCollectionFolderRequest createFolderRequest, CancellationToken cancellationToken)
    {
        if (!IsAuthorized)
            throw new UnauthorizedDiscogsException();
        if (String.IsNullOrWhiteSpace(username))
            throw new ArgumentException(nameof(username));
        if (String.IsNullOrWhiteSpace(createFolderRequest.Name))
            throw new ArgumentException(nameof(createFolderRequest));

        using var request = _oAuthProvider.CreateAuthorizedRequest(HttpMethod.Post, $"{String.Format(DiscogApiUrls.CollectionFoldersUrl, username)}/{folderId}");

        request.Content = CreateJsonContent(createFolderRequest);

        using var response = await _httpClient.SendAsync(request, cancellationToken);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
            throw new UnauthorizedDiscogsException();

        var collectionFolder = response.StatusCode == HttpStatusCode.OK ? await response.Content.DeserializeAsJsonAsync<CollectionFolder>(cancellationToken) : null;

        return collectionFolder;
    }

    public async Task<bool> DeleteCollectionFolderAsync(string username, int folderId, CancellationToken cancellationToken)
    {
        if (!IsAuthorized)
            throw new UnauthorizedDiscogsException();

        using var request = _oAuthProvider.CreateAuthorizedRequest(HttpMethod.Delete, $"{String.Format(DiscogApiUrls.CollectionFoldersUrl, username)}/{folderId}");
        using var response = await _httpClient.SendAsync(request, cancellationToken);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
            throw new UnauthorizedDiscogsException();

        return response.StatusCode == HttpStatusCode.NoContent;
    }
    #endregion


    public void Dispose()
    {
        _httpClient?.Dispose();
    }


    private StringContent CreateJsonContent<T>(T payload)
    {
        string json = payload.SerializeAsJson<T>();

        StringContent stringContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

        return stringContent;
    }
}
