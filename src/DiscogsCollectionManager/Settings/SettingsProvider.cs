using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace DiscogsCollectionManager.Settings;

internal class SettingsProvider : ISettingsProvider
{
    private readonly ISettingsPathProvider _settingsPathProvider;

    public bool SettingsAreLoaded { get; private set; }

    public Settings Settings { get; private set; }

    public SettingsProvider(ISettingsPathProvider settingsPathProvider)
    {
        Settings = new Settings();
        SettingsAreLoaded = false;
        _settingsPathProvider = settingsPathProvider;
    }

    public async Task<bool> SaveSettingsAsync(CancellationToken cancellationToken)
    {
        _settingsPathProvider.EnsureSettingsFolderExists();
        string path = _settingsPathProvider.GetSettingsFilePath();

        using (FileStream fileStream = new FileStream(path, FileMode.Create))
        {
            await JsonSerializer.SerializeAsync(fileStream, Settings, new JsonSerializerOptions { WriteIndented = true }, cancellationToken);
        }

        return true;
    }

    public async Task LoadSettingsAsync(CancellationToken cancellationToken)
    {
        _settingsPathProvider.EnsureSettingsFolderExists();
        string path = _settingsPathProvider.GetSettingsFilePath();
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
