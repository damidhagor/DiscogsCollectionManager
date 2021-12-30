namespace DiscogsCollectionManager.WPF.Utils;

internal interface IPathProvider
{
    string SettingsFilePath { get; }

    string LogFilePath { get; }

    void EnsureAppDataFolderExists();
}
