using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using MediatR;

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
        var result = await _quickAccessRepository.GetAsync(q => q.IsDeleted != false);

        return result.ToList();
    }
}
