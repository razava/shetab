using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using Domain.Models.Relational.IdentityAggregate;
using Microsoft.EntityFrameworkCore;

namespace Application.QuickAccesses.Queries.GetCitizenQuickAccesses;

internal class GetCitizenQuickAccessesQueryHandler(IQuickAccessRepository quickAccessRepository, IUnitOfWork unitOfWork) 
    : IRequestHandler<GetCitizenQuickAccessesQuery, Result<List<QuickAccess>>>
{

    public async Task<Result<List<QuickAccess>>> Handle(GetCitizenQuickAccessesQuery request, CancellationToken cancellationToken)
    {
        var roleIds = await unitOfWork.DbContext.Set<ApplicationRole>()
            .Where(r => request.Roles.Contains(r.Name!))
            .Select(r => r.Id)
            .ToListAsync();

        var result = await quickAccessRepository
            .GetAsync(q => q.IsDeleted == false &&
                roleIds.Contains(q.Category.RoleId) &&
                (request.FilterModel == null || request.FilterModel.Query == null || q.Title.Contains(request.FilterModel.Query)),
                false,
                o => o.OrderBy(q => q.Order));

        return result.ToList();
    }
}
