using Application.Common.Interfaces.Persistence;
using Application.Common.Statics;
using Domain.Models.Relational;
using MediatR;
using System.Linq.Expressions;

namespace Application.QuickAccesses.Queries.GetQuickAccesses;

internal class GetQuickAccessesQueryHandler : IRequestHandler<GetQuickAccessesQuery, List<QuickAccess>>
{
    private readonly IQuickAccessRepository _quickAccessRepository;

    public GetQuickAccessesQueryHandler(IQuickAccessRepository quickAccessRepository)
    {
        _quickAccessRepository = quickAccessRepository;
    }

    public async Task<List<QuickAccess>> Handle(GetQuickAccessesQuery request, CancellationToken cancellationToken)
    {
        Expression<Func<QuickAccess, bool>>? filter = q => q.IsDeleted != true;

        if (request.RoleNames is not null &&  request.RoleNames.Contains(RoleNames.Admin)) 
        {
            filter = null;
        }
        
        var result = await _quickAccessRepository.GetAsync(filter, false, o => o.OrderBy(q => q.Order));

        return result.ToList();
    }
}
