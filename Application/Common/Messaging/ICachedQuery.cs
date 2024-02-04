using MediatR;

namespace Application.Common.Messaging;

public interface ICachedQuery<TResponse> : IRequest<TResponse>, ICachedQuery;
public interface ICachedQuery
{
    string CacheKey { get; }
    TimeSpan? Expiration { get; }
}
