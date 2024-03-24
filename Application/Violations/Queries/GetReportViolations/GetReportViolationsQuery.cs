using Application.Common.Interfaces.Persistence;
using Application.Reports.Common;
using Domain.Models.Relational;
using System.Linq.Expressions;

namespace Application.Violations.Queries.GetReportViolations;

public record GetReportViolationsQuery(int InstanceId, PagingInfo PagingInfo) 
    : IRequest<Result<PagedList<ReportViolationResponse>>>;


public record ReportViolationResponse(
    Guid? ReportId,
    GetReportsResponse Report,
    List<ReportViolationResponseItem> Violations)
{
    public static Expression<Func<IGrouping<Guid?, Violation>, ReportViolationResponse>> GetSelector()
    {
        Expression<Func<IGrouping<Guid?, Violation>, ReportViolationResponse>> selector =
            gv => new ReportViolationResponse(
                gv.Key,
                new GetReportsResponse(
                    gv.First().Report!.Id,
                    gv.First().Report!.ReportState,
                    gv.First().Report!.LastStatus,
                    gv.First().Report!.TrackingNumber,
                    gv.First().Report!.CategoryId,
                    gv.First().Report!.Category.Title,
                    gv.First().Report!.Sent,
                    gv.First().Report!.Deadline,
                    gv.First().Report!.ResponseDeadline,
                    gv.First().Report!.Rating,
                    gv.First().Report!.Priority),
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