namespace Discogs.Client;

internal static class DiscogApiUrls
{
    public static readonly string OAuthBaseUrl = "https://api.discogs.com/oauth";
    public static readonly string OAuthRequestTokenUrl = $"{OAuthBaseUrl}/request_token";
    public static readonly string OAuthAuthorizeUrl = $"{OAuthBaseUrl}/authorize";
    public static readonly string OAuthAccessTokenUrl = $"{OAuthBaseUrl}/access_token";
    public static readonly string OAuthIdentityUrl = $"{OAuthBaseUrl}/identity";
    public static readonly string VerifierTokenUrl = "https://discogs.com/oauth/authorize?oauth_token={0}";
}