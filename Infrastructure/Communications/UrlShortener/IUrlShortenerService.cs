using FluentResults;

namespace Infrastructure.Communications.UrlShortener;

public interface IUrlShortenerService
{
    Task<Result<List<ShortenUrlResponse>>> UrlShortener(List<ShortenUrlRequest> requests);
}

public record ShortenUrlRequest(string Type, string Path);

public record ShortenUrlResponse(string Path, string Url);