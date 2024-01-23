using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using Domain.Models.Relational.Common;
using MediatR;

namespace Application.OrganizationalUnits.Queries.GetOrganizationalUnitsQuery;

internal class GetOrganizationalUnitsQueryHandler(IOrganizationalUnitRepository organizationalUnitRepository) : IRequestHandler<GetOrganizationalUnitsQuery, Result<List<OrganizationalUnit>>>
{

    public async Task<Result<List<OrganizationalUnit>>> Handle(GetOrganizationalUnitsQuery request, CancellationToken cancellationToken)
    {
        var result = await organizationalUnitRepository.GetAsync(
            ou => 
            ou.ShahrbinInstanceId == request.InstanceId 
            && ou.Type == OrganizationalUnitType.OrganizationalUnit
            && ((request.FilterModel == null || request.FilterModel.Query == null) || ou.Title.Contains(request.FilterModel.Query))
            , false);
        return result.ToList();
    }
}
