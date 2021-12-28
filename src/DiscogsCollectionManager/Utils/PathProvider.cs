using System;
using System.IO;

namespace DiscogsCollectionManager.Utils;

internal class PathProvider : IPathProvider
{
    private const string _appDataFolderName = "DiscogsCollectionManager";
    private readonly string _appDataFolderPath;
    private readonly string _settingsFilePath;
    private readonly string _logFolderName = "logs";
    private readonly string _logFolderPath;
    private readonly string _logFilePath;

    public PathProvider()
    {
        _appDataFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), _appDataFolderName);
        _settingsFilePath = Path.Combine(_appDataFolderPath, "settings.json");
        _logFolderPath = Path.Combine(_appDataFolderPath, _logFolderName);
        _logFilePath = Path.Combine(_logFolderPath, "DiscogsCollectionManager.log");
    }

    public string SettingsFilePath => _settingsFilePath;

    public string LogFilePath => _logFilePath;

    public void EnsureAppDataFolderExists()
    {
        Directory.CreateDirectory(_appDataFolderPath);
    }
}