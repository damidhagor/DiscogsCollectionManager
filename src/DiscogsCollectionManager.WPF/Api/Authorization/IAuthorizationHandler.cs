using System.Threading;
using System.Threading.Tasks;
using DiscogsCollectionManager.DiscogsApiClient.Authorization;

namespace DiscogsCollectionManager.WPF.Api.Authorization;

internal interface IAuthorizationHandler
{
    IAuthorizationProvider GetAuthorizationProvider();

    IAuthorizationRequest GetAuthorizationRequest();

    Task SubmitAuthorizationResponseAsync(IAuthorizationResponse authorizationResponse, CancellationToken cancellationToken);
}
