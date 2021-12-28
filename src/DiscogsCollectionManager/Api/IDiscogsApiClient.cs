using System.Threading;
using System.Threading.Tasks;
using Discogs.Client.Contract;
using Discogs.Client.OAuth;

namespace DiscogsCollectionManager.Api;

public interface IDiscogsApiClient
{
    Task<bool> AuthorizeAsync(string verifierCallbackUrl, GetVerifierCallback getVerifierCallback, CancellationToken cancellationToken);

    Task<Identity?> GetIdentityAsync(CancellationToken cancellationToken);

    Task<User?> GetUserAsync(string username, CancellationToken cancellationToken);
}