using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using Microsoft.EntityFrameworkCore;

namespace Application.Violations.Queries.GetReportViolations;

internal class GetReportViolationsQueryHandler(IUnitOfWork unitOfWork) 
    : IRequestHandler<GetReportViolationsQuery, Result<PagedList<ReportViolationResponse>>>
{
    public async Task<Result<PagedList<ReportViolationResponse>>> Handle(GetReportViolationsQuery request, CancellationToken cancellationToken)
    {
        var query = unitOfWork.DbContext.Set<Violation>()
            .AsSingleQuery()
            .AsNoTracking()
            .Where(v => v.ShahrbinInstanceId == request.InstanceId && v.ReportId != null && v.ViolationCheckResult == null)
            .GroupBy(v => v.ReportId)
            .Select(ReportViolationResponse.GetSelector());

        var result = await PagedList<ReportViolationResponse>.ToPagedList(query, request.PagingInfo.PageNumber, request.PagingInfo.PageSize);

        return result;
    }
}
