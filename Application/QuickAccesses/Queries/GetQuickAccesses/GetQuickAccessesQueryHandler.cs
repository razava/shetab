using Application.Common.Interfaces.Persistence;
using Application.QuickAccesses.Common;
using Domain.Models.Relational;
using System.Linq.Expressions;

namespace Application.QuickAccesses.Queries.GetQuickAccesses;

internal class GetQuickAccessesQueryHandler(IQuickAccessRepository quickAccessRepository) 
    : IRequestHandler<GetQuickAccessesQuery, Result<List<AdminGetQuickAccessResponse>>>
{

    public async Task<Result<List<AdminGetQuickAccessResponse>>> Handle(GetQuickAccessesQuery request, CancellationToken cancellationToken)
    {
        Expression<Func<QuickAccess, bool>> filter = q =>
            request.FilterModel == null || request.FilterModel.Query == null ||
            q.Title.Contains(request.FilterModel.Query);

        var result = await quickAccessRepository.GetAll(
            request.InstanceId,
            AdminGetQuickAccessResponse.GetSelector(),
            filter,
            true);

        return result;
    }
}
