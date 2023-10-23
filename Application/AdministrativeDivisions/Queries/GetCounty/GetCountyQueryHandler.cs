using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.Common;
using MediatR;

namespace Application.AdministrativeDivisions.Queries.GetProvince;

internal sealed class GetCountyQueryHandler : IRequestHandler<GetCountyQuery, List<County>>
{
    private readonly ICountyRepository _countyRepository;

    public GetCountyQueryHandler(ICountyRepository countyRepository)
    {
        _countyRepository = countyRepository;
    }

    public async Task<List<County>> Handle(GetCountyQuery request, CancellationToken cancellationToken)
    {
        var result = await _countyRepository.GetAsync(p => p.ProvinceId == request.ProviceId);

        return result.ToList();
    }
}
