using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using DiscogsCollectionManager.Utils;

namespace DiscogsCollectionManager.Settings;

internal class SettingsProvider : ISettingsProvider
{
    private readonly IPathProvider _pathProvider;

    public bool SettingsAreLoaded { get; private set; }

    public Settings Settings { get; private set; }

    public SettingsProvider(IPathProvider pathProvider)
    {
        Settings = new Settings();
        SettingsAreLoaded = false;
        _pathProvider = pathProvider;
    }

    public async Task<bool> SaveSettingsAsync(CancellationToken cancellationToken)
    {
        _pathProvider.EnsureAppDataFolderExists();
        string path = _pathProvider.SettingsFilePath;

        using (FileStream fileStream = new FileStream(path, FileMode.Create))
        {
            await JsonSerializer.SerializeAsync(fileStream, Settings, new JsonSerializerOptions { WriteIndented = true }, cancellationToken);
        }

        return true;
    }

    public async Task LoadSettingsAsync(CancellationToken cancellationToken)
    {
        _pathProvider.EnsureAppDataFolderExists();
        string path = _pathProvider.SettingsFilePath;
        bool settingsExist = false;

        try
        {
            using (FileStream fileStream = new FileStream(path, FileMode.Open))
            {
                var settings = await JsonSerializer.DeserializeAsync<Settings>(fileStream, cancellationToken: cancellationToken);
                if (settings != null)
                {
                    Settings = settings;
                    settingsExist = true;
                }
            }
        }
        catch (FileNotFoundException)
        {
            settingsExist = false;
        }

        if (!settingsExist)
        {
            Settings = new Settings();
            await SaveSettingsAsync(cancellationToken);
        }

        SettingsAreLoaded = true;
    }
}
