using Application.Common.Exceptions;
using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using MediatR;

namespace Application.QuickAccesses.Queries.GetQuickAccessByIdQuery;

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
