using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using System.Linq.Expressions;

namespace Application.Violations.Queries.GetReportViolations;

public record GetReportViolationsQuery(int InstanceId, PagingInfo PagingInfo) : IRequest<Result<PagedList<ReportViolationResponse>>>;


public record ReportViolationResponse(
    Guid? ReportId,
    List<ReportViolationResponseItem> Violations)
{
    public static Expression<Func<IGrouping<Guid?, Violation>, ReportViolationResponse>> GetSelector()
    {
        Expression<Func<IGrouping<Guid?, Violation>, ReportViolationResponse>> selector =
            gv => new ReportViolationResponse(
                gv.Key,
                gv.Select(x => new ReportViolationResponseItem(
                x.Id,
                x.Description,
                x.DateTime,
                x.ViolationType)).ToList());
        return selector;
    }
}

public record ReportViolationResponseItem(
    Guid Id,
    string Description,
    DateTime DateTime,
    ViolationType ViolationType);