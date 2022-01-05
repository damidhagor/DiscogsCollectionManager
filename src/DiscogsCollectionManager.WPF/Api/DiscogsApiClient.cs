using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DiscogsCollectionManager.DiscogsApiClient.Contract;
using DiscogsCollectionManager.DiscogsApiClient.OAuth;
using DiscogsCollectionManager.WPF.Settings;
using Microsoft.Extensions.Configuration;
using DiscogsApi = DiscogsCollectionManager.DiscogsApiClient;

namespace DiscogsCollectionManager.WPF.Api;

public class DiscogsApiClient : IDiscogsApiClient, IDisposable
{
    private const string _userAgent = "MusicLibraryManager/1.0.0";
    private readonly DiscogsApi.DiscogsApiClient _discogsClient;
    private readonly ISettingsProvider _settingsProvider;

    public DiscogsApiClient(ISettingsProvider settingsProvider, IConfiguration configuration)
    {
        _settingsProvider = settingsProvider;

        var consumerKey = configuration.GetValue<string>("ApiSettings:ConsumerKey");
        var consumerSecret = configuration.GetValue<string>("ApiSettings:ConsumerSecret");
        var accessToken = _settingsProvider.Settings.ApiAccessToken;
        var accessTokenSecret = _settingsProvider.Settings.ApiAccessTokenSecret;

        IOAuthProvider oAuthProvider = new PlainOAuthProvider(_userAgent, consumerKey, consumerSecret, accessToken, accessTokenSecret);

        _discogsClient = new DiscogsApi.DiscogsApiClient(oAuthProvider, _userAgent);
    }

    public async Task<bool> AuthorizeAsync(string verifierCallbackUrl, GetVerifierCallback getVerifierCallback, CancellationToken cancellationToken)
    {
        var (success, accessToken, accessTokenSecret) = await _discogsClient.AuthorizeAsync(verifierCallbackUrl, getVerifierCallback, cancellationToken);

        _settingsProvider.Settings.ApiAccessToken = accessToken;
        _settingsProvider.Settings.ApiAccessTokenSecret = accessTokenSecret;
        await _settingsProvider.SaveSettingsAsync(cancellationToken);

        return success;
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
