using Application.Common.Interfaces.Persistence;
using Application.Processes.Common;
using Domain.Models.Relational.ProcessAggregate;
using Microsoft.EntityFrameworkCore;

namespace Application.Processes.Queries.GetProcessById;

internal partial class GetProcessByIdQueryHandler(IUnitOfWork unitOfWork) 
    : IRequestHandler<GetProcessByIdQuery, Result<GetProcessResponse>>
{

    public async Task<Result<GetProcessResponse>> Handle(GetProcessByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await unitOfWork.DbContext.Set<Process>()
            .Where(p => p.Id == request.Id && p.IsDeleted == false)
            .Select(GetProcessResponse.GetSelector())
            .FirstOrDefaultAsync();

        if (result is null)
            return NotFoundErrors.Process;

        return result;
    }
}
