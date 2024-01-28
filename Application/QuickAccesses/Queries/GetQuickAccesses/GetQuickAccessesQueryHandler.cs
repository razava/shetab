using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using MediatR;

namespace Application.QuickAccesses.Queries.GetQuickAccesses;

internal class GetQuickAccessesQueryHandler(IQuickAccessRepository quickAccessRepository) : IRequestHandler<GetQuickAccessesQuery, Result<List<QuickAccess>>>
{

    public async Task<Result<List<QuickAccess>>> Handle(GetQuickAccessesQuery request, CancellationToken cancellationToken)
    {   
        var result = await quickAccessRepository
            .GetAsync(q => (request.ReturnAll || q.IsDeleted == false)
            && ((request.FilterModel == null || request.FilterModel.Query == null) || q.Title.Contains(request.FilterModel.Query))
            , false
            , o => o.OrderBy(q => q.Order));

        return result.ToList();
    }
}
