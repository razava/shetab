using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using Domain.Models.Relational.IdentityAggregate;
using Microsoft.EntityFrameworkCore;

namespace Application.QuickAccesses.Queries.GetCitizenQuickAccesses;

internal class GetCitizenQuickAccessesQueryHandler(IUnitOfWork unitOfWork) 
    : IRequestHandler<GetCitizenQuickAccessesQuery, Result<List<CitizenGetQuickAccessResponse>>>
{

    public async Task<Result<List<CitizenGetQuickAccessResponse>>> Handle(GetCitizenQuickAccessesQuery request, CancellationToken cancellationToken)
    {
        var roleIds = await unitOfWork.DbContext.Set<ApplicationRole>()
            .Where(r => request.Roles.Contains(r.Name!))
            .AsNoTracking()
            .Select(r => r.Id)
            .ToListAsync();

        var result = await unitOfWork.DbContext.Set<QuickAccess>()
            .Where(q => q.IsDeleted == false &&
                roleIds.Contains(q.Category.RoleId) &&
                (request.FilterModel == null || request.FilterModel.Query == null || q.Title.Contains(request.FilterModel.Query)))
            .OrderBy(q => q.Order)
            .Select(CitizenGetQuickAccessResponse.GetSelector())
            .AsNoTracking()
            .ToListAsync();

        return result;
    }
}
