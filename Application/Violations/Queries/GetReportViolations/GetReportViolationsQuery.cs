using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using System.Linq.Expressions;

namespace Application.Violations.Queries.GetReportViolations;

public record GetReportViolationsQuery(Guid ReportId, PagingInfo PagingInfo)
    : IRequest<Result<PagedList<ViolationResponse>>>;

public record ViolationResponse(
    Guid Id,
    string Description,
    DateTime DateTime,
    string ViolationTypeTitle)
{
    public static Expression<Func<Violation, ViolationResponse>> GetSelector()
    {
        Expression<Func<Violation, ViolationResponse>> selector =
            v => new ViolationResponse(
                v.Id, v.Description, v.DateTime, v.ViolationType.Title);
        return selector;
    }
};