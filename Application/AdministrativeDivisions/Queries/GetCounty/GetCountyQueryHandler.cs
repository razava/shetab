using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.Common;

namespace Application.AdministrativeDivisions.Queries.GetCounty;

internal sealed class GetCountyQueryHandler(ICountyRepository countyRepository) : IRequestHandler<GetCountyQuery, Result<List<County>>>
{
    public async Task<Result<List<County>>> Handle(GetCountyQuery request, CancellationToken cancellationToken)
    {
        var result = await countyRepository.GetAsync(p => p.ProvinceId == request.ProviceId);

        return result.ToList();
    }
}
