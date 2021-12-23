namespace Discogs.Client.Contract;

public class Identity
{
    public int Id { get; init; }

    public string Username { get; init; }

    public string ResourceUrl { get; init; }

    public string ConsumerName { get; init; }

    public Identity(int id, string username, string resourceUrl, string consumerName)
    {
        Id = id;
        Username = username;
        ResourceUrl = resourceUrl;
        ConsumerName = consumerName;
    }
}
