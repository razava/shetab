using Application.Common.Interfaces.Persistence;
using Application.QuickAccesses.Common;
using Domain.Models.Relational;
using Microsoft.EntityFrameworkCore;

namespace Application.QuickAccesses.Queries.GetQuickAccessById;

internal sealed class GetQuickAccessByIdQueryHandler(IUnitOfWork unitOfWork) 
    : IRequestHandler<GetQuickAccessByIdQuery, Result<AdminGetQuickAccessResponse>>
{

    public async Task<Result<AdminGetQuickAccessResponse>> Handle(GetQuickAccessByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await unitOfWork.DbContext.Set<QuickAccess>()
            .Where(q => q.Id == request.id)
            .AsNoTracking()
            .Select(AdminGetQuickAccessResponse.GetSelector())
            .FirstOrDefaultAsync();

        if (result == null)
            return NotFoundErrors.QuickAccess;

        return result;
    }
}
