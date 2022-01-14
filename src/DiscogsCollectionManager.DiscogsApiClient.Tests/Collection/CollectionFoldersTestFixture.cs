using System;
using System.Linq;
using System.Threading.Tasks;
using DiscogsCollectionManager.DiscogsApiClient.Exceptions;
using NUnit.Framework;

namespace DiscogsCollectionManager.DiscogsApiClient.Tests.Collection;

public class CollectionFoldersTestFixture : ApiBaseTestFixture
{
    [Test]
    public async Task GetCollectionFolders_Success()
    {
        var username = "damidhagor";

        var foldersResponse = await ApiClient.GetCollectionFoldersAsync(username, default);

        Assert.IsNotNull(foldersResponse);
        Assert.LessOrEqual(2, foldersResponse.Count);

        var allFolder = foldersResponse.FirstOrDefault(f => f.Id == 0);
        var uncategorizedFolder = foldersResponse.FirstOrDefault(f => f.Id == 1);

        Assert.IsNotNull(allFolder);
        Assert.AreEqual(0, allFolder!.Id);
        Assert.AreEqual("All", allFolder!.Name);
        Assert.IsFalse(String.IsNullOrWhiteSpace(allFolder!.ResourceUrl));

        Assert.IsNotNull(uncategorizedFolder);
        Assert.AreEqual(1, uncategorizedFolder!.Id);
        Assert.AreEqual("Uncategorized", uncategorizedFolder!.Name);
        Assert.IsFalse(String.IsNullOrWhiteSpace(uncategorizedFolder!.ResourceUrl));
    }

    [Test]
    public void GetCollectionFolders_EmptyUsername()
    {
        var username = "";

        Assert.ThrowsAsync<ArgumentException>(() => ApiClient.GetCollectionFoldersAsync(username, default), "username");
    }

    [Test]
    public void GetCollectionFolders_InvalidUsername()
    {
        var username = "awrbaerhnqw54";

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.GetCollectionFoldersAsync(username, default));
    }
}