using Application.Common.Interfaces.Map;
using Application.Common.Interfaces.Map.ParsiMap;
using MediatR;

namespace Application.Maps.Queries.MapForwardQuery;

internal class MapForwardQueryHandler(IMapService mapService) : IRequestHandler<MapForwardQuery, Result<ForwardResultApplication>>
{
    public async Task<Result<ForwardResultApplication>> Handle(MapForwardQuery request, CancellationToken cancellationToken)
    {
        var result = await mapService.Forward(request.Address);
        //todo : service itself throw exception bu check for null response possibility
        return result;
    }
}
