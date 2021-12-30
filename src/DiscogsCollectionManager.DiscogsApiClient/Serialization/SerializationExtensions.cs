using System.Text.Json;
using DiscogsCollectionManager.DiscogsApiClient.Exceptions;

namespace DiscogsCollectionManager.DiscogsApiClient.Serialization;

internal static class SerializationExtensions
{
    private static readonly DiscogsJsonNamingPolicy _jsonNamingPolicy = new DiscogsJsonNamingPolicy();
    private static readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
    {
        PropertyNamingPolicy = _jsonNamingPolicy
    };

    public static async Task<T> DeserializeAsJson<T>(this HttpContent httpContent)
    {
        try
        {
            var stringContent = await httpContent.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<T>(stringContent);

            return result ?? throw new SerializationDiscogsException("Deserialization returned no result.");
        }
        catch (Exception ex)
        {
            throw new SerializationDiscogsException("Deserialization failed.", ex);
        }
    }
}