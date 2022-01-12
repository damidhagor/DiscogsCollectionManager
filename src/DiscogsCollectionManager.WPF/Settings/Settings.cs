namespace DiscogsCollectionManager.WPF.Settings;

public class Settings
{
    public string ApiAccessToken { get; set; }
    public string ApiAccessTokenSecret { get; set;}

    public string UserToken { get; set; }

    public Settings()
    {
        ApiAccessToken = "";
        ApiAccessTokenSecret = "";
        UserToken = "";
    }
}
