using Application.Common.Interfaces.Map;
using Application.Common.Interfaces.Map.ParsiMap;
using MediatR;

namespace Application.Maps.Queries.MapBackwardQuery;

internal class MapBackwardQueryHandler : IRequestHandler<MapBackwardQuery, BackwardResultApplication>
{
    private readonly IMapService _mapService;

    public MapBackwardQueryHandler(IMapService mapService)
    {
        _mapService = mapService;
    }

    public async Task<BackwardResultApplication> Handle(MapBackwardQuery request, CancellationToken cancellationToken)
    {
        var result = await _mapService.Backward(request.Longitude, request.Latitude);
        return result;
    }
}
