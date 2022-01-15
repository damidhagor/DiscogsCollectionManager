using System.Net;
using DiscogsCollectionManager.DiscogsApiClient.Authorization;
using DiscogsCollectionManager.DiscogsApiClient.Contract;
using DiscogsCollectionManager.DiscogsApiClient.Exceptions;
using DiscogsCollectionManager.DiscogsApiClient.Serialization;

namespace DiscogsCollectionManager.DiscogsApiClient;

public class DiscogsApiClient
{
    private readonly HttpClient _httpClient;
    private readonly IAuthorizationProvider _authorizationProvider;

    private readonly string _userAgent;

    public bool IsAuthorized => _authorizationProvider.IsAuthorized;

    public DiscogsApiClient(IAuthorizationProvider authorizationProvider, string userAgent)
    {
        _userAgent = userAgent;

        _authorizationProvider = authorizationProvider;

        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(_userAgent);
    }


    public async Task<IAuthorizationResponse> AuthorizeAsync(IAuthorizationRequest authorizationRequest, CancellationToken cancellationToken)
    {
        return await _authorizationProvider.AuthorizeAsync(authorizationRequest, cancellationToken);
    }



    #region User
    public async Task<Identity> GetIdentityAsync(CancellationToken cancellationToken)
    {
        if (!IsAuthorized)
            throw new UnauthorizedDiscogsException();

        using var request = _authorizationProvider.CreateAuthorizedRequest(HttpMethod.Get, DiscogApiUrls.OAuthIdentityUrl);
        using var response = await _httpClient.SendAsync(request, cancellationToken);

        await response.CheckAndHandleHttpErrorCodes(cancellationToken);

        var identity = await response.Content.DeserializeAsJsonAsync<Identity>(cancellationToken);

        return identity;
    }

    public async Task<User> GetUserAsync(string username, CancellationToken cancellationToken)
    {
        if (!IsAuthorized)
            throw new UnauthorizedDiscogsException();
        if (String.IsNullOrWhiteSpace(username))
            throw new ArgumentException(nameof(username));

        using var request = _authorizationProvider.CreateAuthorizedRequest(HttpMethod.Get, String.Format(DiscogApiUrls.UsersUrl, username));
        using var response = await _httpClient.SendAsync(request, cancellationToken);

        await response.CheckAndHandleHttpErrorCodes(cancellationToken);

        var user = await response.Content.DeserializeAsJsonAsync<User>(cancellationToken);

        return user;
    }
    #endregion


    #region Collection Folders
    public async Task<List<CollectionFolder>> GetCollectionFoldersAsync(string username, CancellationToken cancellationToken)
    {
        if (!IsAuthorized)
            throw new UnauthorizedDiscogsException();
        if (String.IsNullOrWhiteSpace(username))
            throw new ArgumentException(nameof(username));

        using var request = _authorizationProvider.CreateAuthorizedRequest(HttpMethod.Get, String.Format(DiscogApiUrls.CollectionFoldersUrl, username));
        using var response = await _httpClient.SendAsync(request, cancellationToken);

        await response.CheckAndHandleHttpErrorCodes(cancellationToken);

        var collectionFoldersResponse = await response.Content.DeserializeAsJsonAsync<CollectionFoldersResponse>(cancellationToken);

        return collectionFoldersResponse.Folders;
    }

    public async Task<CollectionFolder> GetCollectionFolderAsync(string username, int folderId, CancellationToken cancellationToken)
    {
        if (!IsAuthorized)
            throw new UnauthorizedDiscogsException();
        if (String.IsNullOrWhiteSpace(username))
            throw new ArgumentException(nameof(username));

        using var request = _authorizationProvider.CreateAuthorizedRequest(HttpMethod.Get, $"{String.Format(DiscogApiUrls.CollectionFoldersUrl, username)}/{folderId}");
        using var response = await _httpClient.SendAsync(request, cancellationToken);

        await response.CheckAndHandleHttpErrorCodes(cancellationToken);

        var collectionFolder = await response.Content.DeserializeAsJsonAsync<CollectionFolder>(cancellationToken);

        return collectionFolder;
    }

    public async Task<CollectionFolder> CreateCollectionFolderAsync(string username, CreateCollectionFolderRequest createFolderRequest, CancellationToken cancellationToken)
    {
        if (!IsAuthorized)
            throw new UnauthorizedDiscogsException();
        if (String.IsNullOrWhiteSpace(username))
            throw new ArgumentException(nameof(username));
        if (String.IsNullOrWhiteSpace(createFolderRequest.Name))
            throw new ArgumentException(nameof(createFolderRequest));

        using var request = _authorizationProvider.CreateAuthorizedRequest(HttpMethod.Post, String.Format(DiscogApiUrls.CollectionFoldersUrl, username));

        request.Content = CreateJsonContent(createFolderRequest);

        using var response = await _httpClient.SendAsync(request, cancellationToken);

        await response.CheckAndHandleHttpErrorCodes(cancellationToken);

        var collectionFolder = await response.Content.DeserializeAsJsonAsync<CollectionFolder>(cancellationToken);

        return collectionFolder;
    }

    public async Task<CollectionFolder> UpdateCollectionFolderAsync(string username, int folderId, CreateCollectionFolderRequest createFolderRequest, CancellationToken cancellationToken)
    {
        if (!IsAuthorized)
            throw new UnauthorizedDiscogsException();
        if (String.IsNullOrWhiteSpace(username))
            throw new ArgumentException(nameof(username));
        if (String.IsNullOrWhiteSpace(createFolderRequest.Name))
            throw new ArgumentException(nameof(createFolderRequest));

        using var request = _authorizationProvider.CreateAuthorizedRequest(HttpMethod.Post, $"{String.Format(DiscogApiUrls.CollectionFoldersUrl, username)}/{folderId}");

        request.Content = CreateJsonContent(createFolderRequest);

        using var response = await _httpClient.SendAsync(request, cancellationToken);

        await response.CheckAndHandleHttpErrorCodes(cancellationToken);

        var collectionFolder = await response.Content.DeserializeAsJsonAsync<CollectionFolder>(cancellationToken);

        return collectionFolder;
    }

    public async Task<bool> DeleteCollectionFolderAsync(string username, int folderId, CancellationToken cancellationToken)
    {
        if (!IsAuthorized)
            throw new UnauthorizedDiscogsException();
        if (String.IsNullOrWhiteSpace(username))
            throw new ArgumentException(nameof(username));

        using var request = _authorizationProvider.CreateAuthorizedRequest(HttpMethod.Delete, $"{String.Format(DiscogApiUrls.CollectionFoldersUrl, username)}/{folderId}");
        using var response = await _httpClient.SendAsync(request, cancellationToken);

        await response.CheckAndHandleHttpErrorCodes(cancellationToken);

        return response.StatusCode == HttpStatusCode.NoContent;
    }
    #endregion


    #region Database
    public async Task<Artist> GetArtistAsync(int artistId, CancellationToken cancellationToken)
    {
        using var request = _authorizationProvider.CreateAuthorizedRequest(HttpMethod.Get, String.Format(DiscogApiUrls.ArtistsUrl, artistId));
        using var response = await _httpClient.SendAsync(request, cancellationToken);

        await response.CheckAndHandleHttpErrorCodes(cancellationToken);

        var artist = await response.Content.DeserializeAsJsonAsync<Artist>(cancellationToken);

        return artist;
    }


    public async Task<MasterRelease> GetMasterReleaseAsync(int masterReleaseId, CancellationToken cancellationToken)
    {
        using var request = _authorizationProvider.CreateAuthorizedRequest(HttpMethod.Get, String.Format(DiscogApiUrls.MasterReleasesUrl, masterReleaseId));
        using var response = await _httpClient.SendAsync(request, cancellationToken);

        await response.CheckAndHandleHttpErrorCodes(cancellationToken);

        var masterRelease = await response.Content.DeserializeAsJsonAsync<MasterRelease>(cancellationToken);

        return masterRelease;
    }

    public async Task<Release> GetReleaseAsync(int releaseId, CancellationToken cancellationToken)
    {
        using var request = _authorizationProvider.CreateAuthorizedRequest(HttpMethod.Get, String.Format(DiscogApiUrls.ReleasesUrl, releaseId));
        using var response = await _httpClient.SendAsync(request, cancellationToken);

        await response.CheckAndHandleHttpErrorCodes(cancellationToken);

        var release = await response.Content.DeserializeAsJsonAsync<Release>(cancellationToken);

        return release;
    }
    #endregion


    public void Dispose()
    {
        _httpClient?.Dispose();
    }


    private StringContent CreateJsonContent<T>(T payload)
    {
        string json = payload.SerializeAsJson<T>();

        var stringContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

        return stringContent;
    }
}
