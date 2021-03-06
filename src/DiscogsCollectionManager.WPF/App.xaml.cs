using System.Windows;
using DiscogsCollectionManager.WPF.Api;
using DiscogsCollectionManager.WPF.Api.Authorization;
using DiscogsCollectionManager.WPF.Services;
using DiscogsCollectionManager.WPF.Settings;
using DiscogsCollectionManager.WPF.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace DiscogsCollectionManager.WPF;


public partial class App : Application
{
    private readonly IHost _host;

    public App()
    {
        IPathProvider pathProvider = new PathProvider();

        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.File(pathProvider.LogFilePath)
            .CreateLogger();

        _host = new HostBuilder()
            .ConfigureAppConfiguration((context, builder) =>
            {
                builder.SetBasePath(context.HostingEnvironment.ContentRootPath);
                builder.AddJsonFile("appsettings.json");
                builder.AddJsonFile("appsettings.Development.json", true);
            })
            .ConfigureServices((context, services) =>
            {
                services.AddSingleton(pathProvider);
                services.AddSingleton<ISettingsProvider, SettingsProvider>();
                services.AddSingleton(new LoggedInUserService());

                services.AddSingleton<IAuthorizationHandler, PlainOAuthAuthorizationHandler>();
                services.AddSingleton<IDiscogsApiClient, DiscogsCollectionManager.WPF.Api.DiscogsApiClient>();
                services.AddSingleton<MainWindow>();
                services.AddLogging(builder =>
                {
                    builder.AddSerilog();
                });
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
