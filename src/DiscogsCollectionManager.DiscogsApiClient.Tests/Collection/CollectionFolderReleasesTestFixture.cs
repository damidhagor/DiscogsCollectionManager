using System;
using System.Threading.Tasks;
using DiscogsCollectionManager.DiscogsApiClient.Exceptions;
using NUnit.Framework;

namespace DiscogsCollectionManager.DiscogsApiClient.Tests.Collection;

public class CollectionFolderReleasesTestFixture : ApiBaseTestFixture
{
    [Test]
    public async Task GetCollectionFolderReleases_Success()
    {
        var username = "damidhagor";
        var folderId = 1;
        var releaseId = 5134861;

        var response = await ApiClient.AddReleaseToCollectionFolderAsync(username, folderId, releaseId, default);
        

        //Assert.IsNotNull(foldersResponse);
        //Assert.LessOrEqual(2, foldersResponse.Count);

        //var allFolder = foldersResponse.FirstOrDefault(f => f.Id == 0);
        //var uncategorizedFolder = foldersResponse.FirstOrDefault(f => f.Id == 1);

        //Assert.IsNotNull(allFolder);
        //Assert.AreEqual(0, allFolder!.Id);
        //Assert.AreEqual("All", allFolder!.Name);
        //Assert.IsFalse(String.IsNullOrWhiteSpace(allFolder!.ResourceUrl));

        //Assert.IsNotNull(uncategorizedFolder);
        //Assert.AreEqual(1, uncategorizedFolder!.Id);
        //Assert.AreEqual("Uncategorized", uncategorizedFolder!.Name);
        //Assert.IsFalse(String.IsNullOrWhiteSpace(uncategorizedFolder!.ResourceUrl));
    }

    [Test]
    public void GetCollectionFolderReleases_EmptyUsername()
    {
        var username = "";
        var folderId = 0;

        Assert.ThrowsAsync<ArgumentException>(() => ApiClient.GetCollectionFolderReleasesByFolderIdAsync(username, folderId, default), "username");
    }

    [Test]
    public void GetCollectionFolderReleases_InvalidUsername()
    {
        var username = "awrbaerhnqw54";
        var folderId = 0;

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.GetCollectionFolderReleasesByFolderIdAsync(username, folderId, default));
    }

    [Test]
    public void GetCollectionFolderReleases_InvalidFolderId()
    {
        var username = "damidhagor";
        var folderId = -1;

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.GetCollectionFolderReleasesByFolderIdAsync(username, folderId, default));
    }

    [Test]
    public void GetCollectionFolderReleases_NotExistingFolderId()
    {
        var username = "damidhagor";
        var folderId = 42;

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.GetCollectionFolderReleasesByFolderIdAsync(username, folderId, default));
    }


    [Test]
    public void AddReleaseToCollectionFolder_EmptyUsername()
    {
        var username = "";
        var folderId = 1;
        var releaseId = 5134861;

        Assert.ThrowsAsync<ArgumentException>(() => ApiClient.AddReleaseToCollectionFolderAsync(username, folderId, releaseId, default), "username");
    }

    [Test]
    public void AddReleaseToCollectionFolder_InvalidUsername()
    {
        var username = "awrbaerhnqw54";
        var folderId = 1;
        var releaseId = 5134861;

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.AddReleaseToCollectionFolderAsync(username, folderId, releaseId, default));
    }

    [Test]
    public void AddReleaseToCollectionFolder_InvalidFolderId()
    {
        var username = "damidhagor";
        var folderId = -1;
        var releaseId = 5134861;

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.AddReleaseToCollectionFolderAsync(username, folderId, releaseId, default));
    }

    [Test]
    public void AddReleaseToCollectionFolder_NotExistingFolderId()
    {
        var username = "damidhagor";
        var folderId = 42;
        var releaseId = 5134861;

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.AddReleaseToCollectionFolderAsync(username, folderId, releaseId, default));
    }

    [Test]
    public void AddReleaseToCollectionFolder_AllFolderId()
    {
        var username = "damidhagor";
        var folderId = 0;
        var releaseId = 5134861;

        Assert.ThrowsAsync<DiscogsException>(() => ApiClient.AddReleaseToCollectionFolderAsync(username, folderId, releaseId, default),
            "Invalid folder_id: cannot add releases to the 'All' folder.");
    }

    [Test]
    public void AddReleaseToCollectionFolder_InvalidReleaseId()
    {
        var username = "damidhagor";
        var folderId = 1;
        var releaseId = -1;

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.AddReleaseToCollectionFolderAsync(username, folderId, releaseId, default));
    }

    [Test]
    public void AddReleaseToCollectionFolder_NotExistingReleaseId()
    {
        var username = "damidhagor";
        var folderId = 1;
        var releaseId = Int32.MaxValue;

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.AddReleaseToCollectionFolderAsync(username, folderId, releaseId, default));
    }


    [Test]
    public void DeleteReleaseFromCollectionFolder_EmptyUsername()
    {
        var username = "";
        var folderId = 1;
        var releaseId = 5134861;
        var instanceId = -1;

        Assert.ThrowsAsync<ArgumentException>(() => ApiClient.DeleteReleaseFromCollectionFolderAsync(username, folderId, releaseId, instanceId, default), "username");
    }

    [Test]
    public void DeleteReleaseFromCollectionFolder_InvalidUsername()
    {
        var username = "awrbaerhnqw54";
        var folderId = 1;
        var releaseId = 5134861;
        var instanceId = -1;

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.DeleteReleaseFromCollectionFolderAsync(username, folderId, releaseId, instanceId, default));
    }

    [Test]
    public void DeleteReleaseFromCollectionFolder_InvalidFolderId()
    {
        var username = "damidhagor";
        var folderId = -1;
        var releaseId = 5134861;
        var instanceId = -1;

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.DeleteReleaseFromCollectionFolderAsync(username, folderId, releaseId, instanceId, default));
    }

    [Test]
    public void DeleteReleaseFromCollectionFolder_NotExistingFolderId()
    {
        var username = "damidhagor";
        var folderId = 42;
        var releaseId = 5134861;
        var instanceId = -1;

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.DeleteReleaseFromCollectionFolderAsync(username, folderId, releaseId, instanceId, default));
    }

    [Test]
    public void DeleteReleaseFromCollectionFolder_InvalidReleaseId()
    {
        var username = "damidhagor";
        var folderId = 1;
        var releaseId = -1;
        var instanceId = -1;

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.DeleteReleaseFromCollectionFolderAsync(username, folderId, releaseId, instanceId, default));
    }

    [Test]
    public void DeleteReleaseFromCollectionFolder_NotExistingReleaseId()
    {
        var username = "damidhagor";
        var folderId = 1;
        var releaseId = Int32.MaxValue;
        var instanceId = -1;

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.DeleteReleaseFromCollectionFolderAsync(username, folderId, releaseId, instanceId, default));
    }

    [Test]
    public void DeleteReleaseFromCollectionFolder_InvalidInstanceId()
    {
        var username = "damidhagor";
        var folderId = 1;
        var releaseId = 5134861;
        var instanceId = -1;

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.DeleteReleaseFromCollectionFolderAsync(username, folderId, releaseId, instanceId, default));
    }

    [Test]
    public void DeleteReleaseFromCollectionFolder_NotExistingInstanceId()
    {
        var username = "damidhagor";
        var folderId = 1;
        var releaseId = 5134861;
        var instanceId = Int32.MaxValue;

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.DeleteReleaseFromCollectionFolderAsync(username, folderId, releaseId, instanceId, default));
    }
}