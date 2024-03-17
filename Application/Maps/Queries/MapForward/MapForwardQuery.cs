using Application.Common.Interfaces.Map.ParsiMap;

namespace Application.Maps.Queries.MapForwardQuery;

public record MapForwardQuery(string Address) : IRequest<Result<ForwardResultApplication>>;
