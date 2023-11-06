using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using MediatR;

namespace Application.Configurations.Queries.QuickAccesses;

internal sealed class QuickAccessQueryHandler : IRequestHandler<QuickAccessQuery, List<QuickAccess>>
{
    private readonly IQuickAccessRepository _quickAccessRepository;

    public QuickAccessQueryHandler(IQuickAccessRepository quickAccessRepository)
    {
        this._quickAccessRepository = quickAccessRepository;
    }

    public async Task<List<QuickAccess>> Handle(QuickAccessQuery request, CancellationToken cancellationToken)
    {
        var result = await _quickAccessRepository.GetAsync(null, false, o => o.OrderBy(q => q.Order), "Category");
        return result.ToList();
    }
}
