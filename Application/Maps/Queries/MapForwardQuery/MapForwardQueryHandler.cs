using Application.Common.Interfaces.Map;
using Application.Common.Interfaces.Map.ParsiMap;
using MediatR;

namespace Application.Maps.Queries.MapForwardQuery;

internal class MapForwardQueryHandler : IRequestHandler<MapForwardQuery, ForwardResultApplication>
{
    private readonly IMapService _mapService;

    public MapForwardQueryHandler(IMapService mapService)
    {
        _mapService = mapService;
    }

    public async Task<ForwardResultApplication> Handle(MapForwardQuery request, CancellationToken cancellationToken)
    {
        var result = await _mapService.Forward(request.Address);
        return result;
    }
}
