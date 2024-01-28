using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.Common;
using MediatR;

namespace Application.AdministrativeDivisions.Queries.GetRegion;

internal sealed class GetRegionQueryHandler(IRegionRepository regionRepository) : IRequestHandler<GetRegionQuery, Result<List<Region>>>
{

    public async Task<Result<List<Region>>> Handle(GetRegionQuery request, CancellationToken cancellationToken)
    {
        //TODO: perform required filtering
        var result = await regionRepository.GetAsync(p => p.CityId == request.CityId);

        return result.ToList();
    }
}
