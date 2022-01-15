using System;
using System.Threading.Tasks;
using DiscogsCollectionManager.DiscogsApiClient.Exceptions;
using NUnit.Framework;

namespace DiscogsCollectionManager.DiscogsApiClient.Tests.Database;

public class ArtistsTestFixture : ApiBaseTestFixture
{
    [Test]
    public async Task GetArtist_Success()
    {
        var artistId = 287459;

        var artist = await ApiClient.GetArtistAsync(artistId, default);

        Assert.IsNotNull(artist);
        Assert.AreEqual(artistId, artist.Id);
        Assert.AreEqual("HammerFall", artist.Name);
        Assert.IsFalse(String.IsNullOrWhiteSpace(artist.ResourceUrl));
        Assert.IsFalse(String.IsNullOrWhiteSpace(artist.Uri));
        Assert.IsFalse(String.IsNullOrWhiteSpace(artist.ReleasesUrl));
        Assert.IsFalse(String.IsNullOrWhiteSpace(artist.Profile));

        Assert.Less(0, artist.Urls.Count);
        Assert.Less(0, artist.Namevariations.Count);
        Assert.Less(0, artist.Members.Count);
        Assert.Less(0, artist.Images.Count);
    }

    [Test]
    public void GetArtist_NotExistingArtistId()
    {
        var artistId = -1;

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.GetArtistAsync(artistId, default));
    }
}