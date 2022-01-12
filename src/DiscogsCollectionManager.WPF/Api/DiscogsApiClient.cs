using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Auth = DiscogsCollectionManager.DiscogsApiClient.Authorization;
using DiscogsCollectionManager.DiscogsApiClient.Contract;
using DiscogsCollectionManager.WPF.Api.Authorization;
using DiscogsCollectionManager.WPF.Settings;
using Microsoft.Extensions.Configuration;
using GenericDiscogsApiClient = DiscogsCollectionManager.DiscogsApiClient.DiscogsApiClient;

namespace DiscogsCollectionManager.WPF.Api;

internal class DiscogsApiClient : IDiscogsApiClient, IDisposable
{
    public static readonly string UserAgent = "MusicLibraryManager/1.0.0";

    private readonly GenericDiscogsApiClient _discogsClient;
    private readonly IAuthorizationHandler _authorizationHandler;

    public DiscogsApiClient(IAuthorizationHandler authorizationHandler)
    {
        _discogsClient = new GenericDiscogsApiClient(authorizationHandler.GetAuthorizationProvider(), UserAgent);
        _authorizationHandler = authorizationHandler;
    }

    public async Task<bool> AuthorizeAsync(CancellationToken cancellationToken)
    {
        var authorizationRequest = _authorizationHandler.GetAuthorizationRequest();
        var authorizationResponse = await _discogsClient.AuthorizeAsync(authorizationRequest, cancellationToken);

        await _authorizationHandler.SubmitAuthorizationResponseAsync(authorizationResponse, cancellationToken);

        return authorizationResponse.Success;
    }

    public async Task<Identity?> GetIdentityAsync(CancellationToken cancellationToken)
    {
        return await _discogsClient.GetIdentityAsync(cancellationToken);
    }

    public async Task<User?> GetUserAsync(string username, CancellationToken cancellationToken)
    {
        return await _discogsClient.GetUserAsync(username, cancellationToken);
    }


    public async Task<List<CollectionFolder>> GetCollectionFoldersAsync(string username, CancellationToken cancellationToken)
    {
        return await _discogsClient.GetCollectionFoldersAsync(username, cancellationToken);
    }

    public async Task<CollectionFolder?> CreateCollectionFolderAsync(string username, string name, CancellationToken cancellationToken)
    {
        var request = new CreateCollectionFolderRequest(name);
        return await _discogsClient.CreateCollectionFolderAsync(username, request, cancellationToken);
    }

    public async Task<CollectionFolder?> GetCollectionFolderAsync(string username, int folderId, CancellationToken cancellationToken)
    {
        return await _discogsClient.GetCollectionFolderAsync(username, folderId, cancellationToken);
    }

    public async Task<CollectionFolder?> UpdateCollectionFolderAsync(string username, int folderId, string name, CancellationToken cancellationToken)
    {
        return await _discogsClient.UpdateCollectionFolderAsync(username, folderId, new CreateCollectionFolderRequest(name), cancellationToken);
    }

    public async Task<bool> DeleteCollectionFolderAsync(string username, int folderId, CancellationToken cancellationToken)
    {
        return await _discogsClient.DeleteCollectionFolderAsync(username, folderId, cancellationToken);
    }


    public void Dispose()
    {
        _discogsClient.Dispose();
    }
}
