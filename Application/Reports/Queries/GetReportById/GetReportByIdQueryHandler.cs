using Application.Common.Interfaces.Persistence;
using Application.Reports.Common;
using Domain.Models.Relational;
using Microsoft.EntityFrameworkCore;

namespace Application.Reports.Queries.GetReportById;

internal sealed class GetReportByIdQueryHandler(IUnitOfWork unitOfWork) 
    : IRequestHandler<GetReportByIdQuery, Result<GetReportByIdResponse>>
{

    public async Task<Result<GetReportByIdResponse>> Handle(
        GetReportByIdQuery request, CancellationToken cancellationToken)
    {
        var context = unitOfWork.DbContext.Set<Report>();
        var query = context.Where(r => r.Id == request.Id);
        var result = await query
            .AsNoTracking()
            .Select(GetReportByIdResponse.GetSelector())
            .SingleOrDefaultAsync();

        if (result is null)
            return NotFoundErrors.Report;

        return result;
    }
}