using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using Domain.Models.Relational.Common;
using MediatR;

namespace Application.OrganizationalUnits.Queries.GetOrganizationalUnitsQuery;

internal class GetOrganizationalUnitsQueryHandler : IRequestHandler<GetOrganizationalUnitsQuery, List<OrganizationalUnit>>
{
    private readonly IOrganizationalUnitRepository _organizationalUnitRepository;

    public GetOrganizationalUnitsQueryHandler(IOrganizationalUnitRepository organizationalUnitRepository)
    {
        _organizationalUnitRepository = organizationalUnitRepository;
    }

    public async Task<List<OrganizationalUnit>> Handle(GetOrganizationalUnitsQuery request, CancellationToken cancellationToken)
    {
        var result = await _organizationalUnitRepository.GetAsync(
            ou => 
            ou.ShahrbinInstanceId == request.InstanceId 
            && ou.Type == OrganizationalUnitType.OrganizationalUnit
            && ((request.FilterModel == null || request.FilterModel.Query == null) || ou.Title.Contains(request.FilterModel.Query))
            , false);
        return result.ToList();
    }
}
