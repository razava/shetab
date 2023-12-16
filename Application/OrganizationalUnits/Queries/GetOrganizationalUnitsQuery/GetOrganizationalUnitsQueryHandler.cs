using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
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
        var result = await _organizationalUnitRepository.GetAsync(ou => ou.ShahrbinInstanceId == request.InstanceId, false);
        return result.ToList();
    }
}
