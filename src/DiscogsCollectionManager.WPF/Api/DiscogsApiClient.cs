using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Discogs.Client;
using Discogs.Client.Contract;
using Discogs.Client.OAuth;
using DiscogsCollectionManager.WPF.Services;
using DiscogsCollectionManager.WPF.Settings;
using Microsoft.Extensions.Configuration;

namespace DiscogsCollectionManager.WPF.Api;

public class DiscogsApiClient : IDiscogsApiClient, IDisposable
{
    private const string _userAgent = "MusicLibraryManager/1.0.0";
    private readonly DiscogsClient _discogsClient;
    private readonly ISettingsProvider _settingsProvider;

    public DiscogsApiClient(ISettingsProvider settingsProvider, IConfiguration configuration)
    {
        _settingsProvider = settingsProvider;

        var consumerKey = configuration.GetValue<string>("ApiSettings:ConsumerKey");
        var consumerSecret = configuration.GetValue<string>("ApiSettings:ConsumerSecret");
        var accessToken = _settingsProvider.Settings.ApiAccessToken;
        var accessTokenSecret = _settingsProvider.Settings.ApiAccessTokenSecret;

        _discogsClient = new DiscogsClient(_userAgent, consumerKey, consumerSecret, accessToken, accessTokenSecret);
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

    public void Dispose()
    {
        _discogsClient.Dispose();
    }
}
