using System.Threading;
using System.Threading.Tasks;
using DiscogsCollectionManager.DiscogsApiClient.Contract;
using DiscogsCollectionManager.DiscogsApiClient.OAuth;

namespace DiscogsCollectionManager.WPF.Api;

public interface IDiscogsApiClient
{
    Task<bool> AuthorizeAsync(string verifierCallbackUrl, GetVerifierCallback getVerifierCallback, CancellationToken cancellationToken);

    Task<Identity?> GetIdentityAsync(CancellationToken cancellationToken);

    Task<User?> GetUserAsync(string username, CancellationToken cancellationToken);
}