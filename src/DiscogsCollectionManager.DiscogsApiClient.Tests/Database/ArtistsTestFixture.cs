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

    [Test]
    public async Task GetArtistReleases_Success()
    {
        var artistId = 287459;

        var response = await ApiClient.GetArtistReleasesAsync(artistId, 1, 50, default);

        Assert.IsNotNull(response.Pagination);
        Assert.AreEqual(1, response.Pagination.Page);
        Assert.AreEqual(50, response.Pagination.PerPage);
        Assert.Less(0, response.Pagination.Items);
        Assert.Less(0, response.Pagination.Pages);
        Assert.IsNotNull(response.Pagination.Urls);
        Assert.IsFalse(String.IsNullOrWhiteSpace(response.Pagination.Urls.Next));
        Assert.IsFalse(String.IsNullOrWhiteSpace(response.Pagination.Urls.Last));

        Assert.IsNotNull(response.Releases);
        Assert.AreEqual(50, response.Releases.Count);
    }

    [Test]
    public async Task GetArtistReleases_Success_InvalidSmallPageNumber()
    {
        var artistId = 287459;

        var response = await ApiClient.GetArtistReleasesAsync(artistId, -1, 50, default);

        Assert.IsNotNull(response.Pagination);
        Assert.AreEqual(1, response.Pagination.Page);
        Assert.AreEqual(50, response.Pagination.PerPage);
        Assert.Less(0, response.Pagination.Items);
        Assert.Less(0, response.Pagination.Pages);
        Assert.IsNotNull(response.Pagination.Urls);
        Assert.IsFalse(String.IsNullOrWhiteSpace(response.Pagination.Urls.Next));
        Assert.IsFalse(String.IsNullOrWhiteSpace(response.Pagination.Urls.Last));

        Assert.IsNotNull(response.Releases);
        Assert.AreEqual(50, response.Releases.Count);
    }

    [Test]
    public async Task GetArtistReleases_Success_InvalidSmallPageSize()
    {
        var artistId = 287459;

        var response = await ApiClient.GetArtistReleasesAsync(artistId, 1, -1, default);

        Assert.IsNotNull(response.Pagination);
        Assert.AreEqual(1, response.Pagination.Page);
        Assert.AreEqual(1, response.Pagination.PerPage);
        Assert.Less(0, response.Pagination.Items);
        Assert.Less(0, response.Pagination.Pages);
        Assert.IsNotNull(response.Pagination.Urls);
        Assert.IsFalse(String.IsNullOrWhiteSpace(response.Pagination.Urls.Next));
        Assert.IsFalse(String.IsNullOrWhiteSpace(response.Pagination.Urls.Last));

        Assert.IsNotNull(response.Releases);
        Assert.AreEqual(1, response.Releases.Count);
    }

    [Test]
    public async Task GetArtistReleases_Success_InvalidBigPageSize()
    {
        var artistId = 287459;

        var response = await ApiClient.GetArtistReleasesAsync(artistId, 1, 9999999, default);

        Assert.IsNotNull(response.Pagination);
        Assert.AreEqual(1, response.Pagination.Page);
        Assert.AreEqual(100, response.Pagination.PerPage);
        Assert.Less(0, response.Pagination.Items);
        Assert.Less(0, response.Pagination.Pages);
        Assert.IsNotNull(response.Pagination.Urls);
        Assert.IsFalse(String.IsNullOrWhiteSpace(response.Pagination.Urls.Next));
        Assert.IsFalse(String.IsNullOrWhiteSpace(response.Pagination.Urls.Last));

        Assert.IsNotNull(response.Releases);
        Assert.AreEqual(100, response.Releases.Count);
    }

    [Test]
    public async Task GetArtistAllReleases_Success()
    {
        var artistId = 287459;

        var itemCount = 0;
        var summedUpItemCount = 0;

        var response = await ApiClient.GetArtistReleasesAsync(artistId, 1, 50, default);
        itemCount = response.Pagination.Items;
        summedUpItemCount += response.Releases.Count;

        for (int p = 2; p <= response.Pagination.Pages; p++)
        {
            response = await ApiClient.GetArtistReleasesAsync(artistId, p, 50, default);
            summedUpItemCount += response.Releases.Count;
        }

        Assert.AreEqual(itemCount, summedUpItemCount);
    }

    [Test]
    public void GetArtistReleases_NotExistingArtistId()
    {
        var artistId = -1;

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.GetArtistReleasesAsync(artistId, 1, 50, default));
    }

    [Test]
    public void GetArtistReleases_InvalidBigPageNumber()
    {
        var artistId = 287459;

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.GetArtistReleasesAsync(artistId, 9999999, 50, default));
    }
}