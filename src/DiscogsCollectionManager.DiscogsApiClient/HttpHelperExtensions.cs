using System.Net;
using DiscogsCollectionManager.DiscogsApiClient.Contract;
using DiscogsCollectionManager.DiscogsApiClient.Exceptions;
using DiscogsCollectionManager.DiscogsApiClient.Serialization;

namespace DiscogsCollectionManager.DiscogsApiClient;

public static class HttpHelperExtensions
{
    public static async Task CheckAndHandleHttpErrorCodes(this HttpResponseMessage response, CancellationToken cancellationToken)
    {
        if (response.IsSuccessStatusCode)
            return;

        string? message = null;
        try
        {
            var errorMessage = await response.Content.DeserializeAsJsonAsync<ErrorMessage>(cancellationToken);
            message = errorMessage.Message;
        }
        catch { }

        switch (response.StatusCode)
        {
            case HttpStatusCode.Unauthorized:
                throw new UnauthorizedDiscogsException(message);
            case HttpStatusCode.NotFound:
                throw new ResourceNotFoundDiscogsException(message);
            default:
                throw new DiscogsException(message);
        }
    }
}