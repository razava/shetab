using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using MediatR;

namespace Application.OrganizationalUnits.Queries.GetOrganizationalUnitByIdQuery;

internal class GetOrganizationalUnitByIdQueryHandler(IOrganizationalUnitRepository organizationalUnitRepository) : IRequestHandler<GetOrganizationalUnitByIdQuery, Result<OrganizationalUnit>>
{

    public async Task<Result<OrganizationalUnit>> Handle(GetOrganizationalUnitByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await organizationalUnitRepository.GetSingleAsync(
            ou => ou.Id == request.OrganizationalUnitId,
            false,
            "OrganizationalUnits");
        if (result is null)
            return NotFoundErrors.OrganizationalUnit;
        return result;
    }
}
