using Application.Common.Interfaces.Persistence;
using Application.OrganizationalUnits.Common;
using Domain.Models.Relational;
using Domain.Models.Relational.Common;
using Microsoft.EntityFrameworkCore;

namespace Application.OrganizationalUnits.Queries.GetOrganizationalUnits;

internal class GetOrganizationalUnitsQueryHandler(IUnitOfWork unitOfWork) 
    : IRequestHandler<GetOrganizationalUnitsQuery, Result<List<GetOrganizationalUnitListResponse>>>
{

    public async Task<Result<List<GetOrganizationalUnitListResponse>>> Handle(GetOrganizationalUnitsQuery request, CancellationToken cancellationToken)
    {
        var result = await unitOfWork.DbContext.Set<OrganizationalUnit>()
            .Where(ou =>
                ou.ShahrbinInstanceId == request.InstanceId && ou.Type == OrganizationalUnitType.OrganizationalUnit &&
                (request.FilterModel == null || request.FilterModel.Query == null || ou.Title.Contains(request.FilterModel.Query)))
            .Select(GetOrganizationalUnitListResponse.GetSelector())
            .ToListAsync();

        return result.ToList();
    }
}
