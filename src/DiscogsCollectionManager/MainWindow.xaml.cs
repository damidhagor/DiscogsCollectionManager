﻿using System;
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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace DiscogsCollectionManager;

public partial class MainWindow : Window
{
    private readonly IConfiguration _configuration;

    public MainWindow(IConfiguration configuration)
    {
        _configuration = configuration;
        InitializeComponent();
    }



    private async void LoginButton_Click(object sender, RoutedEventArgs e)
    {
        await LoginAsync(default);
    }

    private async Task LoginAsync(CancellationToken cancellationToken)
    {
        var consumerKey = _configuration.GetValue<string>("ApiSettings:ConsumerKey");
        var consumerSecret = _configuration.GetValue<string>("ApiSettings:ConsumerSecret");

        using (var discogsClient = new DiscogsClient("MusicLibraryManager/1.0.0", consumerKey, consumerSecret))
        {
            var success = await discogsClient.AuthorizeAsync("http://musiclibrarymanager/oauth_result", GetUserVerification, cancellationToken);

            if (success)
            {
                var identity = await discogsClient.GetIdentity(cancellationToken);
                var user = await discogsClient.GetUser(identity?.Username, cancellationToken);
                MessageBox.Show(this, $"Logged in successfully as {identity?.Username}!", "Login");
            }
            else
            {
                MessageBox.Show(this, "Login failed!", "Login");
            }
        }
    }

    private Task<string> GetUserVerification(string loginUrl, string callbackUrl, CancellationToken cancellationToken)
    {
        LoginWindow loginWindow = new LoginWindow(loginUrl, callbackUrl);

        loginWindow.ShowDialog();

        return Task.FromResult(loginWindow.Result);
    }
}
