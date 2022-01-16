using System;
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
}