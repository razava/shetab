using Application.Common.Interfaces.Map.ParsiMap;
using MediatR;

namespace Application.Maps.Queries.MapForwardQuery;

public record MapForwardQuery(string Address) : IRequest<Result<ForwardResultApplication>>;
