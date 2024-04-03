using Application.Common.Interfaces.Persistence;
using Application.QuickAccesses.Common;

namespace Application.QuickAccesses.Queries.GetQuickAccessById;

internal sealed class GetQuickAccessByIdQueryHandler(IQuickAccessRepository quickAccessRepository) 
    : IRequestHandler<GetQuickAccessByIdQuery, Result<AdminGetQuickAccessResponse>>
{

    public async Task<Result<AdminGetQuickAccessResponse>> Handle(GetQuickAccessByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await quickAccessRepository.GetById(request.id, AdminGetQuickAccessResponse.GetSelector());

        if (result == null)
            return NotFoundErrors.QuickAccess;

        return result;
    }
}
