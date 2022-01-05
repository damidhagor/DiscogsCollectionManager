namespace DiscogsCollectionManager.DiscogsApiClient.Contract;

/**
 {
  "id": 0,
  "count": 23,
  "name": "All",
  "resource_url": "https://api.discogs.com/users/example/collection/folders/0"
 }
*/

public record CollectionFolder(
    int Id,
    int Count,
    string Name,
    string ResourceUrl);