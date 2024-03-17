using Application.Common.Interfaces.Persistence;
using Mapster;

namespace Application.AdministrativeDivisions.Queries.GetRegion;

internal sealed class GetRegionQueryHandler(IRegionRepository regionRepository) : IRequestHandler<GetRegionQuery, Result<List<GetRegionResponse>>>
{

    public async Task<Result<List<GetRegionResponse>>> Handle(GetRegionQuery request, CancellationToken cancellationToken)
    {
        //TODO: perform required filtering
        var result = await regionRepository.GetAsync(p => p.CityId == request.CityId);

        return result.Adapt<List<GetRegionResponse>>();
    }
}
