using Application.Common.Interfaces.Caching;
using Application.Common.Messaging;
using MediatR;

namespace Application.Common.PipeLineBehaviors;

internal class QueryCachingPipelineBehavior<TRequest, TResponse>(IQueryCacheService cacheService)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICachedQuery
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        return await cacheService.GetOrCreateAsync(
            request.CacheKey,
            _ => next(),
            request.Expiration,
            cancellationToken);
    }
}
