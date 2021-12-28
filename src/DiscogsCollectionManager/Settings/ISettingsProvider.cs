using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DiscogsCollectionManager.Settings;

public interface ISettingsProvider
{
    bool SettingsAreLoaded { get; }

    Settings Settings { get; }


    Task<bool> SaveSettingsAsync(CancellationToken cancellationToken);

    Task LoadSettingsAsync(CancellationToken cancellationToken);
}
