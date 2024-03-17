using Application.Common.Interfaces.Persistence;
using Application.OrganizationalUnits.Common;
using Domain.Models.Relational;
using Microsoft.EntityFrameworkCore;

namespace Application.OrganizationalUnits.Queries.GetOrganizationalUnitByUserId;

internal class GetOrganizationalUnitByUserIdQueryHandler(IUnitOfWork unitOfWork) 
    : IRequestHandler<GetOrganizationalUnitByUserIdQuery, Result<GetOrganizationalUnitResponse>>
{

    public async Task<Result<GetOrganizationalUnitResponse>> Handle(GetOrganizationalUnitByUserIdQuery request, CancellationToken cancellationToken)
    {
        var result = await unitOfWork.DbContext.Set<OrganizationalUnit>()
            .Where(ou => ou.UserId == request.UserId)
            .Select(GetOrganizationalUnitResponse.GetSelector())
            .FirstOrDefaultAsync();

        if (result is null)
            return NotFoundErrors.OrganizationalUnit;
        return result;
    }
}
