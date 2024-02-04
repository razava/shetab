using Application.Common.Interfaces.Caching;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Caching;

internal class QueryCacheService(IMemoryCache memoryCache) : IQueryCacheService
{
    private static readonly TimeSpan DefaultExpiration = TimeSpan.FromMinutes(5);
    public async Task<T> GetOrCreateAsync<T>(
        string key,
        Func<CancellationToken, Task<T>> factory,
        TimeSpan? expiration = null,
        CancellationToken cancellationToken = default)
    {
        T? result = await memoryCache.GetOrCreateAsync(
            key,
            entry =>
            {
                entry.SetAbsoluteExpiration(expiration ?? DefaultExpiration);
                return factory(cancellationToken);
            });
        if (result is null)
            throw new Exception("Null result, not expected!");

        return result;
    }
}
