using System.Threading;
using System.Threading.Tasks;
using DiscogsCollectionManager.DiscogsApiClient.Authorization;
using DiscogsCollectionManager.DiscogsApiClient.Authorization.PlainOAuth;
using DiscogsCollectionManager.WPF.Settings;
using Microsoft.Extensions.Configuration;

namespace DiscogsCollectionManager.WPF.Api.Authorization;

public class PlainOAuthAuthorizationHandler : IAuthorizationHandler
{
    private const string _verifierCallbackUrl = "http://musiclibrarymanager/oauth_result";

    private readonly ISettingsProvider _settingsProvider;
    private readonly PlainOAuthAuthorizationProvider _authorizationProvider;

    public PlainOAuthAuthorizationHandler(ISettingsProvider settingsProvider, IConfiguration configuration)
    {
        _settingsProvider = settingsProvider;

        var consumerKey = configuration.GetValue<string>("ApiSettings:ConsumerKey");
        var consumerSecret = configuration.GetValue<string>("ApiSettings:ConsumerSecret");
        var accessToken = _settingsProvider.Settings.ApiAccessToken;
        var accessTokenSecret = _settingsProvider.Settings.ApiAccessTokenSecret;

        _authorizationProvider = new PlainOAuthAuthorizationProvider(DiscogsApiClient.UserAgent, consumerKey, consumerSecret, accessToken, accessTokenSecret);
    }

    public IAuthorizationProvider GetAuthorizationProvider()
    {
        return _authorizationProvider;
    }

    public IAuthorizationRequest GetAuthorizationRequest()
    {
        return new PlainOAuthAuthorizationRequest(_verifierCallbackUrl, GetVerifier);
    }

    public async Task SubmitAuthorizationResponseAsync(IAuthorizationResponse authorizationResponse, CancellationToken cancellationToken)
    {
        if (authorizationResponse is PlainOAuthAuthorizationResponse plainOAuthResponse
            && plainOAuthResponse.Success
            && plainOAuthResponse.AccessToken is not null
            && plainOAuthResponse.AccessSecret is not null)
        {
            _settingsProvider.Settings.ApiAccessToken = plainOAuthResponse.AccessToken;
            _settingsProvider.Settings.ApiAccessTokenSecret = plainOAuthResponse.AccessSecret;
            await _settingsProvider.SaveSettingsAsync(cancellationToken);
        }
    }


    private Task<string> GetVerifier(string authorizeUrl, string verifierCallbackUrl, CancellationToken cancellationToken)
    {
        var loginWindow = new LoginWindow(authorizeUrl, verifierCallbackUrl);

        loginWindow.ShowDialog();

        return Task.FromResult(loginWindow.Result);
    }
}
