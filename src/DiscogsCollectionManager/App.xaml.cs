using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using DiscogsCollectionManager.Api;
using DiscogsCollectionManager.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DiscogsCollectionManager;


public partial class App : Application
{
    private readonly IHost _host;

    public App()
    {
        _host = new HostBuilder()
            .ConfigureAppConfiguration((context, builder) =>
            {
                builder.SetBasePath(context.HostingEnvironment.ContentRootPath);
                builder.AddJsonFile("appsettings.json");
                builder.AddJsonFile("appsettings.Development.json", true);
            })
            .ConfigureServices((context, services) =>
            {
                services.AddTransient<ISettingsPathProvider, SettingsPathProvider>();
                services.AddSingleton<ISettingsProvider, Settings.SettingsProvider>();

                services.AddSingleton<IDiscogsApiClient, DiscogsApiClient>();
                services.AddSingleton<MainWindow>();
            })
            .Build();
    }

    private async void Application_Startup(object sender, StartupEventArgs e)
    {
        await _host.StartAsync();

        ISettingsProvider settingsProvider = _host.Services.GetRequiredService<ISettingsProvider>();
        await settingsProvider.LoadSettingsAsync(default);

        MainWindow? window = _host.Services.GetService<MainWindow>();
        window?.Show();
    }

    private async void Application_Exit(object sender, ExitEventArgs e)
    {
        await _host.StopAsync();
    }
}
