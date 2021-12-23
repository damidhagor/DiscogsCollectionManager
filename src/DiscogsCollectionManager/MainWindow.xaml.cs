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

namespace DiscogsCollectionManager;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }



    private async void LoginButton_Click(object sender, RoutedEventArgs e)
    {
        await LoginAsync(default);
    }

    private async Task LoginAsync(CancellationToken cancellationToken)
    {
        using (var discogsClient = new DiscogsClient("MusicLibraryManager/1.0.0", "gPLATgUwWxszQAMSVxXC", "zBMFwTzSYXNyMZPMYCxWoVeNaCYVrlbM"))
        {
            var success = await discogsClient.AuthorizeAsync("http://musiclibrarymanager/oauth_result", GetUserVerification, cancellationToken);

            if (success)
            {
                var identity = await discogsClient.GetIdentity(cancellationToken);
                MessageBox.Show(this, $"Logged in successfully!{Environment.NewLine}{Environment.NewLine}{identity}", "Login");
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
