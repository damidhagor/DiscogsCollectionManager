﻿using System;
using System.Threading.Tasks;
using DiscogsCollectionManager.DiscogsApiClient.Exceptions;
using NUnit.Framework;

namespace DiscogsCollectionManager.DiscogsApiClient.Tests.Database;

public class LabelsTestFixture : ApiBaseTestFixture
{
    [Test]
    public async Task GetLabel_Success()
    {
        var labelId = 11499;

        var label = await ApiClient.GetLabelAsync(labelId, default);

        Assert.IsNotNull(label);
        Assert.AreEqual(labelId, label.Id);
        Assert.AreEqual("Nuclear Blast", label.Name);
        Assert.IsFalse(String.IsNullOrWhiteSpace(label.ContactInfo));
        Assert.IsFalse(String.IsNullOrWhiteSpace(label.Profile));
        Assert.IsFalse(String.IsNullOrWhiteSpace(label.ResourceUrl));
        Assert.IsFalse(String.IsNullOrWhiteSpace(label.Uri));
        Assert.IsFalse(String.IsNullOrWhiteSpace(label.ReleasesUrl));
        Assert.Less(0, label.Images.Count);
        Assert.IsNotNull(label.ParentLabel);
        Assert.AreEqual(222987, label.ParentLabel.Id);
        Assert.AreEqual("Nuclear Blast GmbH", label.ParentLabel.Name);
        Assert.IsFalse(String.IsNullOrWhiteSpace(label.ParentLabel.ResourceUrl));
        Assert.Less(0, label.Sublabels.Count);
        Assert.Less(0, label.Urls.Count);
    }

    [Test]
    public void GetLabel_NotExistingLabelId()
    {
        var labelId = -1;

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.GetLabelAsync(labelId, default));
    }

    [Test]
    public async Task GetLabelReleases_Success()
    {
        var labelId = 11499;

        var response = await ApiClient.GetLabelReleasesAsync(labelId, 1, 50, default);

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
    public async Task GetLabelReleases_Success_InvalidSmallPageNumber()
    {
        var labelId = 11499;

        var response = await ApiClient.GetLabelReleasesAsync(labelId, -1, 50, default);

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
    public async Task GetLabelReleases_Success_InvalidSmallPageSize()
    {
        var labelId = 11499;

        var response = await ApiClient.GetLabelReleasesAsync(labelId, 1, -1, default);

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
    public async Task GetLabelReleases_Success_InvalidBigPageSize()
    {
        var labelId = 11499;

        var response = await ApiClient.GetLabelReleasesAsync(labelId, 1, 9999999, default);

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
    public async Task GetLabelAllReleases_Success()
    {
        var labelId = 34650;

        var itemCount = 0;
        var summedUpItemCount = 0;

        var response = await ApiClient.GetLabelReleasesAsync(labelId, 1, 50, default);
        itemCount = response.Pagination.Items;
        summedUpItemCount += response.Releases.Count;

        for (int p = 2; p <= response.Pagination.Pages; p++)
        {
            response = await ApiClient.GetLabelReleasesAsync(labelId, p, 50, default);
            summedUpItemCount += response.Releases.Count;
        }

        Assert.AreEqual(itemCount, summedUpItemCount);
    }

    [Test]
    public void GetLabelReleases_NotExistingLabelId()
    {
        var labelId = -1;

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.GetLabelReleasesAsync(labelId, 1, 50, default));
    }

    [Test]
    public void GetLabelReleases_InvalidBigPageNumber()
    {
        var labelId = 11499;

        Assert.ThrowsAsync<ResourceNotFoundDiscogsException>(() => ApiClient.GetLabelReleasesAsync(labelId, 9999999, 50, default));
    }
}