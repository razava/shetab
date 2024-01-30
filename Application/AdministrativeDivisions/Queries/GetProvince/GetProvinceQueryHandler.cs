using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using Domain.Models.Relational.Common;
using MediatR;

namespace Application.AdministrativeDivisions.Queries.GetProvince;

internal sealed class GetProvinceQueryHandler(IProvinceRepository provinceRepository) : IRequestHandler<GetProvinceQuery, Result<List<Province>>>
{
    public async Task<Result<List<Province>>> Handle(GetProvinceQuery request, CancellationToken cancellationToken)
    {
        //TODO: perform required filtering
        var result = await provinceRepository.GetAsync();

        return result.ToList();
    }
}
