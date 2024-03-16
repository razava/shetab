using Application.Common.Interfaces.Map;
using Application.Common.Interfaces.Map.ParsiMap;
using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.Common;
using Microsoft.EntityFrameworkCore;

namespace Application.Maps.Queries.MapBackwardQuery;

internal class MapBackwardQueryHandler(IUnitOfWork unitOfWork, IMapService mapService) : IRequestHandler<MapBackwardQuery, Result<AddressResult>>
{
    public async Task<Result<AddressResult>> Handle(MapBackwardQuery request, CancellationToken cancellationToken)
    {
        BackwardResultApplication result;
        try
        {
            result = await mapService.Backward(request.Longitude, request.Latitude);
        }
        catch
        {
            return MapErrors.AddressResolutionFailed;
        }

        if (result is null)
            return MapErrors.AddressResolutionFailed;

        var cityId = unitOfWork.DbContext.Set<ShahrbinInstance>()
            .Where(i => i.Id == request.instanceId)
            .Select(i => i.CityId)
            .First();

        var regions = await unitOfWork.DbContext.Set<Region>()
            .Where(r => r.CityId == cityId)
            .ToListAsync();

        var regionName = result?.Geofences?.FirstOrDefault()?.Title;
        var region = regions.Where(r => r.Name == regionName).FirstOrDefault();

        int regionId;
        if(regionName is null || region is null || regions.Count == 1)
        {
            regionId = regions[0].Id;
        }
        else
        {
            regionId = region.Id;
        }

        return new AddressResult(request.instanceId, result?.Address ?? "", regionId);
    }
}
