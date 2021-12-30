﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using DiscogsCollectionManager.WPF.Api;
using DiscogsCollectionManager.WPF.Services;
using DiscogsCollectionManager.WPF.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DiscogsCollectionManager.WPF;

public partial class MainWindow : Window, INotifyPropertyChanged
{
    #region INotifyPropertyChanged
    public event PropertyChangedEventHandler PropertyChanged;
    private void NotifyPropertyChanged([CallerMemberName] string caller = "")
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
    #endregion

    private readonly ILogger _log;
    private readonly IDiscogsApiClient _discogsApiClient;

    public LoggedInUserService LoggedInUserService { get; init; }

    public MainWindow(ILogger<MainWindow> log, IDiscogsApiClient discogsApiClient, LoggedInUserService loggedInUserService)
    {
        _log = log;
        _discogsApiClient = discogsApiClient;
        LoggedInUserService = loggedInUserService;
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

            if (user != null)
            {
                LoggedInUserService.SetLoggedInUser(user);
                NotifyPropertyChanged(nameof(LoggedInUserService));
            }

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

public class MyClass : INotifyPropertyChanged
{
    #region INotifyPropertyChanged
    public event PropertyChangedEventHandler PropertyChanged;
    private void NotifyPropertyChanged([CallerMemberName] string caller = "")
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
    #endregion


}