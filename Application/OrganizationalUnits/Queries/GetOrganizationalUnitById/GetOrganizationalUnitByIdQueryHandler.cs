using Application.Common.Interfaces.Persistence;
using Application.OrganizationalUnits.Common;

namespace Application.OrganizationalUnits.Queries.GetOrganizationalUnitById;

internal class GetOrganizationalUnitByIdQueryHandler(IOrganizationalUnitRepository organizationalUnitRepository) 
    : IRequestHandler<GetOrganizationalUnitByIdQuery, Result<GetOrganizationalUnitResponse>>
{

    public async Task<Result<GetOrganizationalUnitResponse>> Handle(GetOrganizationalUnitByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await organizationalUnitRepository.GetOrganizationalUnitById(
            request.OrganizationalUnitId,
            GetOrganizationalUnitResponse.GetSelector());

        if (result is null)
            return NotFoundErrors.OrganizationalUnit;

        return result;
    }
}
