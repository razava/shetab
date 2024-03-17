using Application.Common.Interfaces.Persistence;
using Application.OrganizationalUnits.Common;
using Domain.Models.Relational;
using Microsoft.EntityFrameworkCore;

namespace Application.OrganizationalUnits.Queries.GetOrganizationalUnitById;

internal class GetOrganizationalUnitByIdQueryHandler(IUnitOfWork unitOfWork) 
    : IRequestHandler<GetOrganizationalUnitByIdQuery, Result<GetOrganizationalUnitResponse>>
{

    public async Task<Result<GetOrganizationalUnitResponse>> Handle(GetOrganizationalUnitByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await unitOfWork.DbContext.Set<OrganizationalUnit>()
            .Where(ou => ou.Id == request.OrganizationalUnitId)
            .Select(GetOrganizationalUnitResponse.GetSelector())
            .FirstOrDefaultAsync();

        if (result is null)
            return NotFoundErrors.OrganizationalUnit;

        return result;
    }
}
