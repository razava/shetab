using Application.Common.Interfaces.Persistence;
using Application.QuickAccesses.Common;
using Domain.Models.Relational;
using Microsoft.EntityFrameworkCore;

namespace Application.QuickAccesses.Queries.GetQuickAccesses;

internal class GetQuickAccessesQueryHandler(IUnitOfWork unitOfWork) 
    : IRequestHandler<GetQuickAccessesQuery, Result<List<AdminGetQuickAccessResponse>>>
{

    public async Task<Result<List<AdminGetQuickAccessResponse>>> Handle(GetQuickAccessesQuery request, CancellationToken cancellationToken)
    {   
        var result = await unitOfWork.DbContext.Set<QuickAccess>()
            .Where(q => q.IsDeleted == false &&
                (request.FilterModel == null || request.FilterModel.Query == null || q.Title.Contains(request.FilterModel.Query)))
            .AsNoTracking()
            .OrderBy(q => q.Order)
            .Select(AdminGetQuickAccessResponse.GetSelector())
            .ToListAsync();

        return result;
    }
}
