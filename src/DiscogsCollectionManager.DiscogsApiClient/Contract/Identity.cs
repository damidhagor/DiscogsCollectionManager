using System.Text.Json.Serialization;

namespace DiscogsCollectionManager.DiscogsApiClient.Contract;

/**
 {
  "id": 1,
  "username": "example",
  "resource_url": "https://api.discogs.com/users/example",
  "consumer_name": "Your Application Name"
 }
*/

public class Identity
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("username")]
    public string Username { get; init; }

    [JsonPropertyName("resource_url")]
    public string ResourceUrl { get; init; }

    [JsonPropertyName("consumer_name")]
    public string ConsumerName { get; init; }

    public Identity(int id, string username, string resourceUrl, string consumerName)
    {
        Id = id;
        Username = username;
        ResourceUrl = resourceUrl;
        ConsumerName = consumerName;
    }
}
