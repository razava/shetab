using Application.Common.Interfaces.Persistence;
using Application.Reports.Common;
using Domain.Models.Relational;
using Microsoft.EntityFrameworkCore;

namespace Application.Violations.Queries.GetReportViolationsList;

internal class GetReportViolationsListQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetReportViolationsListQuery, Result<PagedList<GetReportsResponse>>>
{
    public async Task<Result<PagedList<GetReportsResponse>>> Handle(GetReportViolationsListQuery request, CancellationToken cancellationToken)
    {
        var query = unitOfWork.DbContext.Set<Report>()
            .AsNoTracking()
            .Where(r => r.ShahrbinInstanceId == request.InstanceId && r.ViolationCount>0 && !r.IsViolationChecked)
            .Select(GetReportsResponse.GetSelector());

        var result = await PagedList<GetReportsResponse>.ToPagedList(query, request.PagingInfo.PageNumber, request.PagingInfo.PageSize);

        return result;
    }
}
