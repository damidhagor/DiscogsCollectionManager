using System;
using System.Threading.Tasks;
using DiscogsCollectionManager.DiscogsApiClient.QueryParameters;
using NUnit.Framework;

namespace DiscogsCollectionManager.DiscogsApiClient.Tests.Database;

public class SearchTestFixture : ApiBaseTestFixture
{
    [Test]
    public async Task Search_Success()
    {
        var query = "hammerfall";
        var paginationParams = new PaginationQueryParameters(1, 50);

        var response = await ApiClient.SearchDatabaseAsync(query, paginationParams, default);

        Assert.IsNotNull(response);
        Assert.IsNotNull(response.Pagination);
        Assert.AreEqual(1, response.Pagination.Page);
        Assert.Less(0, response.Pagination.Items);
        Assert.Less(0, response.Pagination.Pages);
        Assert.Less(0, response.Pagination.PerPage);
        Assert.IsFalse(String.IsNullOrWhiteSpace(response.Pagination.Urls.Next));
        Assert.IsFalse(String.IsNullOrWhiteSpace(response.Pagination.Urls.Last));
        Assert.AreEqual(50, response.Results.Count);
    }

    [Test]
    public async Task Search_NoQuery_Success()
    {
        var query = "";
        var paginationParams = new PaginationQueryParameters(1, 50);

        var response = await ApiClient.SearchDatabaseAsync(query, paginationParams, default);

        Assert.IsNotNull(response);
        Assert.IsNotNull(response.Pagination);
        Assert.AreEqual(1, response.Pagination.Page);
        Assert.Less(0, response.Pagination.Items);
        Assert.Less(0, response.Pagination.Pages);
        Assert.Less(0, response.Pagination.PerPage);
        Assert.IsFalse(String.IsNullOrWhiteSpace(response.Pagination.Urls.Next));
        Assert.IsFalse(String.IsNullOrWhiteSpace(response.Pagination.Urls.Last));
        Assert.AreEqual(50, response.Results.Count);
    }


    [Test]
    public async Task Search_Success_InvalidSmallPageNumber()
    {
        var query = "hammerfall";
        var paginationParams = new PaginationQueryParameters(-1, 50);

        var response = await ApiClient.SearchDatabaseAsync(query, paginationParams, default);

        Assert.IsNotNull(response.Pagination);
        Assert.AreEqual(1, response.Pagination.Page);
        Assert.AreEqual(50, response.Pagination.PerPage);
        Assert.Less(0, response.Pagination.Items);
        Assert.Less(0, response.Pagination.Pages);
        Assert.IsNotNull(response.Pagination.Urls);
        Assert.IsFalse(String.IsNullOrWhiteSpace(response.Pagination.Urls.Next));
        Assert.IsFalse(String.IsNullOrWhiteSpace(response.Pagination.Urls.Last));
        Assert.AreEqual(50, response.Results.Count);
    }

    [Test]
    public async Task Search_Success_InvalidSmallPageSize()
    {
        var query = "hammerfall";
        var paginationParams = new PaginationQueryParameters(1, -1);

        var response = await ApiClient.SearchDatabaseAsync(query, paginationParams, default);

        Assert.IsNotNull(response.Pagination);
        Assert.AreEqual(1, response.Pagination.Page);
        Assert.AreEqual(1, response.Pagination.PerPage);
        Assert.Less(0, response.Pagination.Items);
        Assert.Less(0, response.Pagination.Pages);
        Assert.IsNotNull(response.Pagination.Urls);
        Assert.IsFalse(String.IsNullOrWhiteSpace(response.Pagination.Urls.Next));
        Assert.IsFalse(String.IsNullOrWhiteSpace(response.Pagination.Urls.Last));
        Assert.AreEqual(1, response.Results.Count);
    }

    [Test]
    public async Task Search_Success_InvalidBigPageSize()
    {
        var query = "hammerfall";
        var paginationParams = new PaginationQueryParameters(1, int.MaxValue);

        var response = await ApiClient.SearchDatabaseAsync(query, paginationParams, default);

        Assert.IsNotNull(response.Pagination);
        Assert.AreEqual(1, response.Pagination.Page);
        Assert.AreEqual(100, response.Pagination.PerPage);
        Assert.Less(0, response.Pagination.Items);
        Assert.Less(0, response.Pagination.Pages);
        Assert.IsNotNull(response.Pagination.Urls);
        Assert.IsFalse(String.IsNullOrWhiteSpace(response.Pagination.Urls.Next));
        Assert.IsFalse(String.IsNullOrWhiteSpace(response.Pagination.Urls.Last));
        Assert.AreEqual(100, response.Results.Count);
    }
}