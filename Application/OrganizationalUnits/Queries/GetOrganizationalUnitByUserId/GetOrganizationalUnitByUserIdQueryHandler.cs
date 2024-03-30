using Application.Common.Interfaces.Persistence;
using Application.OrganizationalUnits.Common;

namespace Application.OrganizationalUnits.Queries.GetOrganizationalUnitByUserId;

internal class GetOrganizationalUnitByUserIdQueryHandler(IOrganizationalUnitRepository organizationalUnitRepository) 
    : IRequestHandler<GetOrganizationalUnitByUserIdQuery, Result<GetOrganizationalUnitResponse>>
{

    public async Task<Result<GetOrganizationalUnitResponse>> Handle(GetOrganizationalUnitByUserIdQuery request, CancellationToken cancellationToken)
    {
        //var result = await unitOfWork.DbContext.Set<OrganizationalUnit>()
        //    .Where(ou => ou.UserId == request.UserId)
        //    .Select(GetOrganizationalUnitResponse.GetSelector())
        //    .FirstOrDefaultAsync();
        var result = await organizationalUnitRepository.GetOrganizationalUnitByUserId(
            request.UserId,
            GetOrganizationalUnitResponse.GetSelector());

        if (result is null)
            return NotFoundErrors.OrganizationalUnit;

        return result;
    }
}
