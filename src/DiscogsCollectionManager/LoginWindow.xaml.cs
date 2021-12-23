using System;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Web.WebView2.Core;

namespace DiscogsCollectionManager;

public partial class LoginWindow : Window
{
    private readonly string _loginUrl;
    private readonly string _callbackUrl;

    public string? Result { get; set; }

    public LoginWindow(string loginUrl, string callbackUrl)
    {
        _loginUrl = loginUrl;
        _callbackUrl = callbackUrl;

        InitializeComponent();
    }


    private void Browser_Navigated(object sender, NavigationEventArgs e)
    {
        Uri uri = e.Uri;

        if (uri.AbsoluteUri.Contains("oauth_verifier"))
        {
            Result = uri.Query.Length > 0 ? uri.Query.Substring(1) : "";
        }
    }

    private async void Browser_Loaded(object sender, RoutedEventArgs e)
    {
        await Browser.EnsureCoreWebView2Async();
        Browser.Source = new Uri(_loginUrl);
    }

    private void Browser_NavigationStarting(object sender, CoreWebView2NavigationStartingEventArgs e)
    {
        if (e.Uri.StartsWith(_callbackUrl))
        {
            int index = e.Uri.IndexOf("?");

            if (index > -1)
                Result = e.Uri.Substring(index + 1);
            else
                Result = null;

            Close();
        }
    }
}
