using System;
using System.IO;

namespace DiscogsCollectionManager.Settings;

internal class SettingsPathProvider : ISettingsPathProvider
{
    private const string _settingsFolderName = "DiscogsCollectionManager";
    private readonly string _settingsFolderPath;
    private readonly string _settingsFolderFilePath;

    public SettingsPathProvider()
    {
        _settingsFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), _settingsFolderName);
        _settingsFolderFilePath = Path.Combine(_settingsFolderPath, "settings.json");
    }

    public string GetSettingsFilePath() => _settingsFolderFilePath;

    public void EnsureSettingsFolderExists()
    {
        Directory.CreateDirectory(_settingsFolderPath);
    }
}