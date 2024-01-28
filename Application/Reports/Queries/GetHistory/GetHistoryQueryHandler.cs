using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Reports.Queries.GetReportById;

internal sealed class GetHistoryQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetHistoryQuery, Result<List<TransitionLog>>>
{
    public async Task<Result<List<TransitionLog>>> Handle(GetHistoryQuery request, CancellationToken cancellationToken)
    {
        //TODO: check whether user can access to content or not
        var context = unitOfWork.DbContext;
        var result = await context.Set<TransitionLog>().Where(tl => tl.ReportId == request.Id).ToListAsync(cancellationToken);
        return result;
    }
}