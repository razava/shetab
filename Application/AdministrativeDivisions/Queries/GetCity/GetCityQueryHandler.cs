using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.Common;
using MediatR;

namespace Application.AdministrativeDivisions.Queries.GetProvince;

internal sealed class GetCityQueryHandler(ICityRepository cityRepository) : IRequestHandler<GetCityQuery, Result<List<City>>>
{
    public async Task<Result<List<City>>> Handle(GetCityQuery request, CancellationToken cancellationToken)
    {
        var result = await cityRepository.GetAsync(p => p.DistrictId == request.DistrictId);

        return result.ToList();
    }
}
