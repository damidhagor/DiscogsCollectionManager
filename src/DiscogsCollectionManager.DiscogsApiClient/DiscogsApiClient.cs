﻿using System.Net;
using DiscogsCollectionManager.DiscogsApiClient.Authorization;
using DiscogsCollectionManager.DiscogsApiClient.Contract;
using DiscogsCollectionManager.DiscogsApiClient.Exceptions;
using DiscogsCollectionManager.DiscogsApiClient.QueryParameters;
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

        using var request = _authorizationProvider.CreateAuthorizedRequest(HttpMethod.Get, DiscogsApiUrls.OAuthIdentityUrl);
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

        using var request = _authorizationProvider.CreateAuthorizedRequest(HttpMethod.Get, String.Format(DiscogsApiUrls.UsersUrl, username));
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

        using var request = _authorizationProvider.CreateAuthorizedRequest(HttpMethod.Get, String.Format(DiscogsApiUrls.CollectionFoldersUrl, username));
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

        using var request = _authorizationProvider.CreateAuthorizedRequest(HttpMethod.Get, $"{String.Format(DiscogsApiUrls.CollectionFoldersUrl, username)}/{folderId}");
        using var response = await _httpClient.SendAsync(request, cancellationToken);

        await response.CheckAndHandleHttpErrorCodes(cancellationToken);

        var collectionFolder = await response.Content.DeserializeAsJsonAsync<CollectionFolder>(cancellationToken);

        return collectionFolder;
    }

    public async Task<CollectionFolder> CreateCollectionFolderAsync(string username, string folderName, CancellationToken cancellationToken)
    {
        if (!IsAuthorized)
            throw new UnauthorizedDiscogsException();
        if (String.IsNullOrWhiteSpace(username))
            throw new ArgumentException(nameof(username));
        if (String.IsNullOrWhiteSpace(folderName))
            throw new ArgumentException(nameof(folderName));

        using var request = _authorizationProvider.CreateAuthorizedRequest(HttpMethod.Post, String.Format(DiscogsApiUrls.CollectionFoldersUrl, username));

        request.Content = CreateJsonContent(new { Name = folderName });

        using var response = await _httpClient.SendAsync(request, cancellationToken);

        await response.CheckAndHandleHttpErrorCodes(cancellationToken);

        var collectionFolder = await response.Content.DeserializeAsJsonAsync<CollectionFolder>(cancellationToken);

        return collectionFolder;
    }

    public async Task<CollectionFolder> UpdateCollectionFolderAsync(string username, int folderId, string folderName, CancellationToken cancellationToken)
    {
        if (!IsAuthorized)
            throw new UnauthorizedDiscogsException();
        if (String.IsNullOrWhiteSpace(username))
            throw new ArgumentException(nameof(username));
        if (String.IsNullOrWhiteSpace(folderName))
            throw new ArgumentException(nameof(folderName));

        using var request = _authorizationProvider.CreateAuthorizedRequest(HttpMethod.Post, $"{String.Format(DiscogsApiUrls.CollectionFoldersUrl, username)}/{folderId}");

        request.Content = CreateJsonContent(new { Name = folderName });

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

        using var request = _authorizationProvider.CreateAuthorizedRequest(HttpMethod.Delete, $"{String.Format(DiscogsApiUrls.CollectionFoldersUrl, username)}/{folderId}");
        using var response = await _httpClient.SendAsync(request, cancellationToken);

        await response.CheckAndHandleHttpErrorCodes(cancellationToken);

        return response.StatusCode == HttpStatusCode.NoContent;
    }
    #endregion


    #region Collection Items
    public async Task<CollectionFolderReleasesResponse> GetCollectionFolderReleasesByFolderIdAsync(string username, int folderId, CancellationToken cancellationToken)
    {
        if (!IsAuthorized)
            throw new UnauthorizedDiscogsException();
        if (String.IsNullOrWhiteSpace(username))
            throw new ArgumentException(nameof(username));

        using var request = _authorizationProvider.CreateAuthorizedRequest(HttpMethod.Get, String.Format(DiscogsApiUrls.CollectionFolderReleasesUrl, username, folderId));
        using var response = await _httpClient.SendAsync(request, cancellationToken);

        await response.CheckAndHandleHttpErrorCodes(cancellationToken);

        var collectionFolderReleasesResponse = await response.Content.DeserializeAsJsonAsync<CollectionFolderReleasesResponse>(cancellationToken);

        return collectionFolderReleasesResponse;
    }

    public async Task<CollectionFolderRelease> AddReleaseToCollectionFolderAsync(string username, int folderId, int releaseId, CancellationToken cancellationToken)
    {
        if (!IsAuthorized)
            throw new UnauthorizedDiscogsException();
        if (String.IsNullOrWhiteSpace(username))
            throw new ArgumentException(nameof(username));

        using var request = _authorizationProvider.CreateAuthorizedRequest(HttpMethod.Post, String.Format(DiscogsApiUrls.CollectionFolderAddReleaseUrl, username, folderId, releaseId));
        using var response = await _httpClient.SendAsync(request, cancellationToken);

        await response.CheckAndHandleHttpErrorCodes(cancellationToken);

        var collectionFolderRelease = await response.Content.DeserializeAsJsonAsync<CollectionFolderRelease>(cancellationToken);

        return collectionFolderRelease;
    }

    public async Task<bool> DeleteReleaseFromCollectionFolderAsync(string username, int folderId, int releaseId, int instanceId, CancellationToken cancellationToken)
    {
        if (!IsAuthorized)
            throw new UnauthorizedDiscogsException();
        if (String.IsNullOrWhiteSpace(username))
            throw new ArgumentException(nameof(username));

        using var request = _authorizationProvider.CreateAuthorizedRequest(HttpMethod.Delete, String.Format(DiscogsApiUrls.CollectionFolderDeleteReleaseUrl, username, folderId, releaseId, instanceId));
        using var response = await _httpClient.SendAsync(request, cancellationToken);

        await response.CheckAndHandleHttpErrorCodes(cancellationToken);

        return response.StatusCode == HttpStatusCode.NoContent;
    }
    #endregion


    #region Wantlist
    public async Task<WantlistReleasesResponse> GetWantlistReleasesAsync(string username, PaginationQueryParameters paginationQueryParameters, CancellationToken cancellationToken)
    {
        if (!IsAuthorized)
            throw new UnauthorizedDiscogsException();
        if (String.IsNullOrWhiteSpace(username))
            throw new ArgumentException(nameof(username));

        string url = QueryParameterHelper.AppendPaginationQueryParameters(String.Format(DiscogsApiUrls.WantlistUrl, username), paginationQueryParameters);

        using var request = _authorizationProvider.CreateAuthorizedRequest(HttpMethod.Get, url);
        using var response = await _httpClient.SendAsync(request, cancellationToken);

        await response.CheckAndHandleHttpErrorCodes(cancellationToken);

        var releasesResponse = await response.Content.DeserializeAsJsonAsync<WantlistReleasesResponse>(cancellationToken);

        return releasesResponse;
    }

    public async Task<WantlistRelease> AddWantlistReleaseAsync(string username, int releaseId, CancellationToken cancellationToken)
    {
        if (!IsAuthorized)
            throw new UnauthorizedDiscogsException();
        if (String.IsNullOrWhiteSpace(username))
            throw new ArgumentException(nameof(username));

        using var request = _authorizationProvider.CreateAuthorizedRequest(HttpMethod.Put, String.Format(DiscogsApiUrls.WantlistReleaseUrl, username, releaseId));
        using var response = await _httpClient.SendAsync(request, cancellationToken);

        await response.CheckAndHandleHttpErrorCodes(cancellationToken);

        var release = await response.Content.DeserializeAsJsonAsync<WantlistRelease>(cancellationToken);

        return release;
    }

    public async Task<bool> DeleteWantlistReleaseAsync(string username, int releaseId, CancellationToken cancellationToken)
    {
        if (!IsAuthorized)
            throw new UnauthorizedDiscogsException();
        if (String.IsNullOrWhiteSpace(username))
            throw new ArgumentException(nameof(username));

        using var request = _authorizationProvider.CreateAuthorizedRequest(HttpMethod.Delete, String.Format(DiscogsApiUrls.WantlistReleaseUrl, username, releaseId));
        using var response = await _httpClient.SendAsync(request, cancellationToken);

        await response.CheckAndHandleHttpErrorCodes(cancellationToken);

        return response.StatusCode == HttpStatusCode.NoContent;
    }
    #endregion


    #region Database
    public async Task<Artist> GetArtistAsync(int artistId, CancellationToken cancellationToken)
    {
        using var request = _authorizationProvider.CreateAuthorizedRequest(HttpMethod.Get, String.Format(DiscogsApiUrls.ArtistsUrl, artistId));
        using var response = await _httpClient.SendAsync(request, cancellationToken);

        await response.CheckAndHandleHttpErrorCodes(cancellationToken);

        var artist = await response.Content.DeserializeAsJsonAsync<Artist>(cancellationToken);

        return artist;
    }

    public async Task<ArtistReleasesResponse> GetArtistReleasesAsync(int artistId, PaginationQueryParameters paginationQueryParameters, CancellationToken cancellationToken)
    {
        string url = QueryParameterHelper.AppendPaginationQueryParameters(String.Format(DiscogsApiUrls.ArtistReleasesUrl, artistId), paginationQueryParameters);

        using var request = _authorizationProvider.CreateAuthorizedRequest(HttpMethod.Get, url);
        using var response = await _httpClient.SendAsync(request, cancellationToken);

        await response.CheckAndHandleHttpErrorCodes(cancellationToken);

        var releasesResponse = await response.Content.DeserializeAsJsonAsync<ArtistReleasesResponse>(cancellationToken);

        return releasesResponse;
    }


    public async Task<MasterRelease> GetMasterReleaseAsync(int masterReleaseId, CancellationToken cancellationToken)
    {
        using var request = _authorizationProvider.CreateAuthorizedRequest(HttpMethod.Get, String.Format(DiscogsApiUrls.MasterReleasesUrl, masterReleaseId));
        using var response = await _httpClient.SendAsync(request, cancellationToken);

        await response.CheckAndHandleHttpErrorCodes(cancellationToken);

        var masterRelease = await response.Content.DeserializeAsJsonAsync<MasterRelease>(cancellationToken);

        return masterRelease;
    }

    public async Task<Release> GetReleaseAsync(int releaseId, CancellationToken cancellationToken)
    {
        using var request = _authorizationProvider.CreateAuthorizedRequest(HttpMethod.Get, String.Format(DiscogsApiUrls.ReleasesUrl, releaseId));
        using var response = await _httpClient.SendAsync(request, cancellationToken);

        await response.CheckAndHandleHttpErrorCodes(cancellationToken);

        var release = await response.Content.DeserializeAsJsonAsync<Release>(cancellationToken);

        return release;
    }

    public async Task<ReleaseCommunityRatingResponse> GetReleaseCommunityRatingAsync(int releaseId, CancellationToken cancellationToken)
    {
        using var request = _authorizationProvider.CreateAuthorizedRequest(HttpMethod.Get, String.Format(DiscogsApiUrls.ReleaseCommunityRatingsUrl, releaseId));
        using var response = await _httpClient.SendAsync(request, cancellationToken);

        await response.CheckAndHandleHttpErrorCodes(cancellationToken);

        var rating = await response.Content.DeserializeAsJsonAsync<ReleaseCommunityRatingResponse>(cancellationToken);

        return rating;
    }


    public async Task<Label> GetLabelAsync(int labelId, CancellationToken cancellationToken)
    {
        using var request = _authorizationProvider.CreateAuthorizedRequest(HttpMethod.Get, String.Format(DiscogsApiUrls.LabelsUrl, labelId));
        using var response = await _httpClient.SendAsync(request, cancellationToken);

        await response.CheckAndHandleHttpErrorCodes(cancellationToken);

        var label = await response.Content.DeserializeAsJsonAsync<Label>(cancellationToken);

        return label;
    }

    public async Task<LabelReleasesResponse> GetLabelReleasesAsync(int labelId, PaginationQueryParameters paginationQueryParameters, CancellationToken cancellationToken)
    {
        string url = QueryParameterHelper.AppendPaginationQueryParameters(String.Format(DiscogsApiUrls.LabelReleasesUrl, labelId), paginationQueryParameters);

        using var request = _authorizationProvider.CreateAuthorizedRequest(HttpMethod.Get, url);
        using var response = await _httpClient.SendAsync(request, cancellationToken);

        await response.CheckAndHandleHttpErrorCodes(cancellationToken);

        var releasesResponse = await response.Content.DeserializeAsJsonAsync<LabelReleasesResponse>(cancellationToken);

        return releasesResponse;
    }


    public async Task<SearchResultsResponse> SearchDatabaseAsync(string query, PaginationQueryParameters paginationQueryParameters, CancellationToken cancellationToken)
    {
        string url = $"{DiscogsApiUrls.SearchUrl}?q={query}&{paginationQueryParameters.CreateQueryParameterString()}";

        using var request = _authorizationProvider.CreateAuthorizedRequest(HttpMethod.Get, url);
        using var response = await _httpClient.SendAsync(request, cancellationToken);

        await response.CheckAndHandleHttpErrorCodes(cancellationToken);

        var content = await response.Content.ReadAsStringAsync();
        var searchResultsResponse = await response.Content.DeserializeAsJsonAsync<SearchResultsResponse>(cancellationToken);

        return searchResultsResponse;
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
