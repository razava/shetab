using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.Common;
using MediatR;

namespace Application.AdministrativeDivisions.Queries.GetProvince;

internal sealed class GetCityQueryHandler : IRequestHandler<GetCityQuery, List<City>>
{
    private readonly ICityRepository _cityRepository;

    public GetCityQueryHandler(ICityRepository cityRepository)
    {
        _cityRepository = cityRepository;
    }

    public async Task<List<City>> Handle(GetCityQuery request, CancellationToken cancellationToken)
    {
        var result = await _cityRepository.GetAsync(p => p.DistrictId == request.DistrictId);

        return result.ToList();
    }
}
