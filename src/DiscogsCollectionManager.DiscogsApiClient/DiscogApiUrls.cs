namespace DiscogsCollectionManager.DiscogsApiClient;

internal static class DiscogApiUrls
{
    public static readonly string BaseUrl = "https://api.discogs.com";

    public static readonly string OAuthBaseUrl = $"{BaseUrl}/oauth";
    public static readonly string OAuthRequestTokenUrl = $"{OAuthBaseUrl}/request_token";
    public static readonly string OAuthAuthorizeUrl = $"{OAuthBaseUrl}/authorize";
    public static readonly string OAuthAccessTokenUrl = $"{OAuthBaseUrl}/access_token";
    public static readonly string OAuthIdentityUrl = $"{OAuthBaseUrl}/identity";
    public static readonly string VerifierTokenUrl = "https://discogs.com/oauth/authorize?oauth_token={0}";

    public static readonly string UsersUrl = $"{BaseUrl}/users/{{0}}";
    public static readonly string CollectionFoldersUrl = $"{BaseUrl}/users/{{0}}/collection/folders";

    public static readonly string ArtistsUrl = $"{BaseUrl}/artists/{{0}}";
    public static readonly string MasterReleasesUrl = $"{BaseUrl}/masters/{{0}}";
    public static readonly string ReleasesUrl = $"{BaseUrl}/releases/{{0}}";
    public static readonly string ReleaseCommunityRatingsUrl = $"{BaseUrl}/releases/{{0}}/rating";
    public static readonly string LabelsUrl = $"{BaseUrl}/labels/{{0}}";
    public static readonly string LabelReleasesUrl = $"{BaseUrl}/labels/{{0}}/releases";

    public static string CreatePaginationQuery(int page, int perPage) => $"?page={Math.Max(1, page)}&per_page={Math.Clamp(perPage, 1, 100)}";
}