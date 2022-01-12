using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DiscogsCollectionManager.DiscogsApiClient.Contract;

namespace DiscogsCollectionManager.WPF.Api;

public interface IDiscogsApiClient
{
    Task<bool> AuthorizeAsync(CancellationToken cancellationToken);

    Task<Identity?> GetIdentityAsync(CancellationToken cancellationToken);

    Task<User?> GetUserAsync(string username, CancellationToken cancellationToken);


    Task<List<CollectionFolder>> GetCollectionFoldersAsync(string username, CancellationToken cancellationToken);

    Task<CollectionFolder?> CreateCollectionFolderAsync(string username, string name, CancellationToken cancellationToken);

    Task<CollectionFolder?> GetCollectionFolderAsync(string username, int folderId, CancellationToken cancellationToken);

    Task<CollectionFolder?> UpdateCollectionFolderAsync(string username, int folderId, string name, CancellationToken cancellationToken);

    Task<bool> DeleteCollectionFolderAsync(string username, int folderId, CancellationToken cancellationToken);
}