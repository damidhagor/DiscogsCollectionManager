﻿using System;
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

    //[Test]
    //public void CreateCollectionFolder_EmptyUsername()
    //{
    //    var username = "";
    //    var folderName = "API_TEST_CREATE_EMPTY_USERNAME";

    //    Assert.ThrowsAsync<ArgumentException>(() => ApiClient.CreateCollectionFolderAsync(username, folderName, default), "username");
    //}

    //[Test]
    //public void CreateCollectionFolder_InvalidUsername()
    //{
    //    var username = "awrbaerhnqw54";
    //    var folderName = "API_TEST_CREATE_INVALID_USERNAME";

    //    Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.CreateCollectionFolderAsync(username, folderName, default));
    //}

    //[Test]
    //public void CreateCollectionFolder_EmptyFolderName()
    //{
    //    var username = "damidhagor";
    //    var folderName = "";

    //    Assert.ThrowsAsync<ArgumentException>(() => ApiClient.CreateCollectionFolderAsync(username, folderName, default), "createFolderRequest");
    //}


    //[Test]
    //public void UpdateCollectionFolder_EmptyUsername()
    //{
    //    var username = "";
    //    var folderId = 0;
    //    var folderName = "API_TEST_UPDATE_EMPTY_USERNAME";

    //    Assert.ThrowsAsync<ArgumentException>(() => ApiClient.UpdateCollectionFolderAsync(username, folderId, folderName, default), "username");
    //}

    //[Test]
    //public void UpdateCollectionFolder_InvalidUsername()
    //{
    //    var username = "awrbaerhnqw54";
    //    var folderId = 0;
    //    var folderName = "API_TEST_UPDATE_INVALID_USERNAME";

    //    Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.UpdateCollectionFolderAsync(username, folderId, folderName, default));
    //}

    //[Test]
    //public void UpdateCollectionFolder_EmptyFolderName()
    //{
    //    var username = "damidhagor";
    //    var folderId = 0;
    //    var folderName = "";

    //    Assert.ThrowsAsync<ArgumentException>(() => ApiClient.UpdateCollectionFolderAsync(username, folderId, folderName, default), "createFolderRequest");
    //}

    //[Test]
    //public void UpdateCollectionFolder_InvalidFolderId()
    //{
    //    var username = "damidhagor";
    //    var folderId = -1;
    //    var folderName = "API_TEST_UPDATE_INVALID_ID";

    //    Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.UpdateCollectionFolderAsync(username, folderId, folderName, default));
    //}

    //[Test]
    //public void UpdateCollectionFolder_NotExistingFolderId()
    //{
    //    var username = "damidhagor";
    //    var folderId = 42;
    //    var folderName = "API_TEST_UPDATE_NOT_EXISTING_ID";

    //    Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.UpdateCollectionFolderAsync(username, folderId, folderName, default));
    //}


    //[Test]
    //public void DeleteCollectionFolder_EmptyUsername()
    //{
    //    var username = "";
    //    var folderId = -1;

    //    Assert.ThrowsAsync<ArgumentException>(() => ApiClient.DeleteCollectionFolderAsync(username, folderId, default), "username");
    //}

    //[Test]
    //public void DeleteCollectionFolder_InvalidUsername()
    //{
    //    var username = "awrbaerhnqw54";
    //    var folderId = 0;

    //    Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.DeleteCollectionFolderAsync(username, folderId, default));
    //}

    //[Test]
    //public void DeleteCollectionFolder_InvalidFolderId()
    //{
    //    var username = "damidhagor";
    //    var folderId = -1;

    //    Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.DeleteCollectionFolderAsync(username, folderId, default));
    //}

    //[Test]
    //public void DeleteCollectionFolder_NotExistingFolderId()
    //{
    //    var username = "damidhagor";
    //    var folderId = 42;

    //    Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.DeleteCollectionFolderAsync(username, folderId, default));
    //}


    //[Test]
    //public async Task CreateUpdateDeleteCollectionFolder_Success()
    //{
    //    var username = "damidhagor";
    //    var folderName1 = "API_TEST_WORKFLOW_CREATE";
    //    var folderName2 = "API_TEST_WORKFLOW_UPDATE";

    //    // Add
    //    var createdFolder = await ApiClient.CreateCollectionFolderAsync(username, folderName1, default);
    //    Assert.IsNotNull(createdFolder);
    //    Assert.AreEqual(folderName1, createdFolder!.Name);

    //    // Update
    //    var updatedFolder = await ApiClient.UpdateCollectionFolderAsync(username, createdFolder.Id, folderName2, default);
    //    Assert.IsNotNull(updatedFolder);
    //    Assert.AreEqual(folderName2, updatedFolder!.Name);
    //    Assert.AreEqual(createdFolder.Id, updatedFolder!.Id);

    //    // Delete
    //    var result = await ApiClient.DeleteCollectionFolderAsync(username, createdFolder.Id, default);
    //    Assert.IsTrue(result);
    //}
}