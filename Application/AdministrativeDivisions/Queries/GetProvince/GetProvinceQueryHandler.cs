using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using Domain.Models.Relational.Common;
using MediatR;

namespace Application.AdministrativeDivisions.Queries.GetProvince;

internal sealed class GetProvinceQueryHandler : IRequestHandler<GetProvinceQuery, List<Province>>
{
    private readonly IProvinceRepository _provinceRepository;

    public GetProvinceQueryHandler(IProvinceRepository provinceRepository)
    {
        _provinceRepository = provinceRepository;
    }

    public async Task<List<Province>> Handle(GetProvinceQuery request, CancellationToken cancellationToken)
    {
        //TODO: perform required filtering
        var result = await _provinceRepository.GetAsync();

        return result.ToList();
    }
}
