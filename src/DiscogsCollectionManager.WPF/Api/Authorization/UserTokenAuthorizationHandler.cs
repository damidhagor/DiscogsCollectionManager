using System.Threading;
using System.Threading.Tasks;
using DiscogsCollectionManager.DiscogsApiClient.Authorization;
using DiscogsCollectionManager.DiscogsApiClient.Authorization.UserToken;
using DiscogsCollectionManager.WPF.Settings;

namespace DiscogsCollectionManager.WPF.Api.Authorization;

public class UserTokenAuthorizationHandler : IAuthorizationHandler
{
    private readonly ISettingsProvider _settingsProvider;
    private readonly UserTokenAuthorizationProvider _authorizationProvider;
    private string _userToken;

    public UserTokenAuthorizationHandler(ISettingsProvider settingsProvider)
    {
        _settingsProvider = settingsProvider;

        _userToken = _settingsProvider.Settings.UserToken;

        _authorizationProvider = new UserTokenAuthorizationProvider();
    }

    public IAuthorizationProvider GetAuthorizationProvider()
    {
        return _authorizationProvider;
    }

    public IAuthorizationRequest GetAuthorizationRequest()
    {
        return new UserTokenAuthorizationRequest(_userToken);
    }


    public Task SubmitAuthorizationResponseAsync(IAuthorizationResponse authorizationResponse, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

}