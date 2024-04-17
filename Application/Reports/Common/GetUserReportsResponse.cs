using Domain.Models.Relational;
using Domain.Models.Relational.Common;
using System.Linq.Expressions;

namespace Application.Reports.Common;

public record GetUserReportsResponse(
    Guid Id,
    ReportState ReportState,
    string LastStatus,
    string TrackingNumber,
    int CategoryId,
    string CategoryTitle,
    DateTime Sent,
    string AddressDetail,
    Priority Priority
    )
{
    public static GetUserReportsResponse FromReport(Report report)
    {
        return new GetUserReportsResponse(
                report.Id,
                report.ReportState,
                report.LastStatus,
                report.TrackingNumber,
                report.CategoryId,
                report.Category.Title,
                report.Sent,
                report.Address.Detail,
                report.Priority);
    }

    public static Expression<Func<Report, GetUserReportsResponse>> GetSelector()
    {
        Expression<Func<Report, GetUserReportsResponse>> selector
            = report => new GetUserReportsResponse(
                report.Id,
                report.ReportState,
                report.LastStatus,
                report.TrackingNumber,
                report.CategoryId,
                report.Category.Title,
                report.Sent,
                report.Address.Detail,
                report.Priority);
        return selector;
    }

}
