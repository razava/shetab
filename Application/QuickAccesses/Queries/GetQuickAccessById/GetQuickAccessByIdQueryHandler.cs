using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;

namespace Application.QuickAccesses.Queries.GetQuickAccessById;

internal sealed class GetQuickAccessByIdQueryHandler(IQuickAccessRepository quickAccessRepository) : IRequestHandler<GetQuickAccessByIdQuery, Result<QuickAccess>>
{

    public async Task<Result<QuickAccess>> Handle(GetQuickAccessByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await quickAccessRepository
            .GetSingleAsync(q => q.Id == request.id, false);
        if (result == null)
            return NotFoundErrors.QuickAccess;
        return result;
    }
}
