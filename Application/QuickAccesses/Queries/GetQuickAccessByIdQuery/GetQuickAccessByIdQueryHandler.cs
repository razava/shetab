using Application.Common.Exceptions;
using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using MediatR;

namespace Application.QuickAccesses.Queries.GetQuickAccessByIdQuery;

internal sealed class GetQuickAccessByIdQueryHandler : IRequestHandler<GetQuickAccessByIdQuery, QuickAccess>
{
    private readonly IQuickAccessRepository _quickAccessRepository;
    
    public GetQuickAccessByIdQueryHandler(IQuickAccessRepository quickAccessRepository)
    {
        _quickAccessRepository = quickAccessRepository;
    }
    public async Task<QuickAccess> Handle(GetQuickAccessByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _quickAccessRepository
            .GetSingleAsync(q => q.Id == request.id, false);
        if (result == null)
            throw new NotFoundException("QuickAccess");
        return result;
    }
}
