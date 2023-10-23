using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.Common;
using MediatR;

namespace Application.AdministrativeDivisions.Queries.GetProvince;

internal sealed class GetDistrictQueryHandler : IRequestHandler<GetDistrictQuery, List<District>>
{
    private readonly IDistrictRepository _districtRepository;

    public GetDistrictQueryHandler(IDistrictRepository districtRepository)
    {
        _districtRepository = districtRepository;
    }

    public async Task<List<District>> Handle(GetDistrictQuery request, CancellationToken cancellationToken)
    {
        var result = await _districtRepository.GetAsync(p => p.CountyId == request.CountyId);

        return result.ToList();
    }
}
