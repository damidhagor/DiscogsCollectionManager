namespace DiscogsCollectionManager.DiscogsApiClient.QueryParameters;

internal static class QueryParameterHelper
{
    public static string AppendPaginationQueryParameters(string url, PaginationQueryParameters paginationQueryParameters)
    {
        if (string.IsNullOrWhiteSpace(url))
            throw new ArgumentNullException(nameof(url));

        string query = paginationQueryParameters.CreateQueryParameterString();

        return string.IsNullOrWhiteSpace(query) ? url : $"{url}?{query}";
    }
}