using Application.Common.Interfaces.Map;
using Application.Common.Interfaces.Map.ParsiMap;
using MediatR;

namespace Application.Maps.Queries.MapBackwardQuery;

internal class MapBackwardQueryHandler(IMapService mapService) : IRequestHandler<MapBackwardQuery, Result<BackwardResultApplication>>
{
    public async Task<Result<BackwardResultApplication>> Handle(MapBackwardQuery request, CancellationToken cancellationToken)
    {
        var result = await mapService.Backward(request.Longitude, request.Latitude);
        //todo : service itself throw exception bu check for null response possibility
        return result;
    }
}
