using FluentResults;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SharedKernel.Errors;
using System.Net.Http.Json;

namespace Infrastructure.Communications.UrlShortener;

public class UrlShortenerService(
    HttpClient httpClient,
    ILogger<UrlShortenerService> logger,
    IOptions<UrlShortenerOptions> urlShortenerOptions) : IDisposable, IUrlShortenerService
{

    public async Task<Result<List<ShortenUrlResponse>>> UrlShortener(List<ShortenUrlRequest> requests)
    {
        try
        {
            var url = @"/url/";
            using HttpResponseMessage response = await httpClient.PostAsJsonAsync(url, requests);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<List<ShortenUrlResponse>>();

            if (result is null)
                return UrlShortenerErrors.General;

            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message, ex);
            return UrlShortenerErrors.General;
        }
    }

    public void Dispose() => httpClient?.Dispose();
}

