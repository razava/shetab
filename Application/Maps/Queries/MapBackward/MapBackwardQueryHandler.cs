using Application.Common.Interfaces.Map;
using Application.Common.Interfaces.Map.ParsiMap;
using Application.Common.Interfaces.Persistence;

namespace Application.Maps.Queries.MapBackward;

internal class MapBackwardQueryHandler(
    IMapService mapService,
    IShahrbinInstanceRepository shahrbinInstanceRepository,
    IRegionRepository regionRepository) : IRequestHandler<MapBackwardQuery, Result<AddressResult>>
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

        var cityId = (await shahrbinInstanceRepository.GetById(request.instanceId)).CityId;

        var regions = await regionRepository.GetRegionsByCityId(cityId);

        var regionName = result?.Geofences?.FirstOrDefault()?.Title;
        var region = regions.Where(r => r.Name == regionName).FirstOrDefault();

        int regionId;
        if (regionName is null || region is null || regions.Count == 1)
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
