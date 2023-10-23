using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.Common;
using MediatR;

namespace Application.AdministrativeDivisions.Queries.GetProvince;

internal sealed class GetRegionQueryHandler : IRequestHandler<GetRegionQuery, List<Region>>
{
    private readonly IRegionRepository _regionRepository;

    public GetRegionQueryHandler(IRegionRepository regionRepository)
    {
        _regionRepository = regionRepository;
    }

    public async Task<List<Region>> Handle(GetRegionQuery request, CancellationToken cancellationToken)
    {
        //TODO: perform required filtering
        var result = await _regionRepository.GetAsync(p => p.CityId == request.CityId);

        return result.ToList();
    }
}
