namespace DiscogsCollectionManager.Settings;

internal interface ISettingsPathProvider
{
    string GetSettingsFilePath();

    void EnsureSettingsFolderExists();
}
