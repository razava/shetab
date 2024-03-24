using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using System.Linq.Expressions;

namespace Application.Violations.Queries.GetReportViolations;

public record GetReportViolationsQuery(Guid ReportId, PagingInfo PagingInfo)
    : IRequest<Result<PagedList<ReportViolationResponse>>>;

public record ReportViolationResponse(
    Guid Id,
    string Description,
    DateTime DateTime,
    string ViolationTypeTitle)
{
    public static Expression<Func<Violation, ReportViolationResponse>> GetSelector()
    {
        Expression<Func<Violation, ReportViolationResponse>> selector =
            v => new ReportViolationResponse(
                v.Id, v.Description, v.DateTime, v.ViolationType.Title);
        return selector;
    }
};