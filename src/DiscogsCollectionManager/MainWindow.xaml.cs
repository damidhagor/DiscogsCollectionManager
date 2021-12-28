using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Discogs.Client;
using DiscogsCollectionManager.Api;
using DiscogsCollectionManager.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DiscogsCollectionManager;

public partial class MainWindow : Window
{
    private readonly ILogger _log;
    private readonly IDiscogsApiClient _discogsApiClient;

    public MainWindow(ILogger<MainWindow> log, IDiscogsApiClient discogsApiClient)
    {
        _log = log;
        _discogsApiClient = discogsApiClient;

        InitializeComponent();
    }



    private async void LoginButton_Click(object sender, RoutedEventArgs e)
    {
        await LoginAsync(default);
    }

    private async Task LoginAsync(CancellationToken cancellationToken)
    {
        _log.LogInformation("Logging in ...");
        var success = await _discogsApiClient.AuthorizeAsync("http://musiclibrarymanager/oauth_result", GetUserVerification, cancellationToken);

        if (success)
        {
            var identity = await _discogsApiClient.GetIdentityAsync(cancellationToken);
            var user = await _discogsApiClient.GetUserAsync(identity?.Username, cancellationToken);
            MessageBox.Show(this, $"Logged in successfully as {identity?.Username}!", "Login");
            _log.LogInformation("Logged in.");
        }
        else
        {
            _log.LogError("Login failed.");
            MessageBox.Show(this, "Login failed!", "Login");
        }
    }

    private Task<string> GetUserVerification(string loginUrl, string callbackUrl, CancellationToken cancellationToken)
    {
        LoginWindow loginWindow = new LoginWindow(loginUrl, callbackUrl);

        loginWindow.ShowDialog();

        return Task.FromResult(loginWindow.Result);
    }
}
