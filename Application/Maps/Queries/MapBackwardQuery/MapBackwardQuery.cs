using Application.Common.Interfaces.Map.ParsiMap;
using MediatR;

namespace Application.Maps.Queries.MapBackwardQuery;

public record MapBackwardQuery(double Longitude, double Latitude):IRequest<BackwardResultApplication>;
