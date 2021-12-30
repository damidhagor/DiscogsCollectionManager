using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discogs.Client.Contract;

namespace DiscogsCollectionManager.WPF.Services;

public class LoggedInUserService
{
    public LoggedInUser? LoggedInUser { get; private set; }


    public void SetLoggedInUser(User user)
    {
        LoggedInUser = new(user);
    }

    public void UnsetLoggedUser()
    {
        LoggedInUser = null;
    }
}


public class LoggedInUser
{
    public int Id { get; init; }

    public string Username { get; init; }

    public float Rank { get; init; }

    public string CollectionFoldersUrl { get; init; }

    public string WantlistUrl { get; init; }

    public string AvatarUrl { get; init; }

    public string Email { get; init; }


    public LoggedInUser(User user)
    {
        Id = user.Id;
        Username = user.Username;
        Rank = user.Rank;
        CollectionFoldersUrl = user.CollectionFoldersUrl;
        WantlistUrl = user.WantlistUrl;
        AvatarUrl = user.AvatarUrl;
        Email = user.Email;
    }
}