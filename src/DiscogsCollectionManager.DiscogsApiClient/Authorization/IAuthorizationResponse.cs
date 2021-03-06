namespace DiscogsCollectionManager.DiscogsApiClient.Authorization;

public interface IAuthorizationResponse
{
    bool Success { get; }

    string? Error { get; }
}