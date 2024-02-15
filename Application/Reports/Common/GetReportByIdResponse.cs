using Domain.Models.Relational;
using Domain.Models.Relational.Common;
using System.Linq.Expressions;

namespace Application.Reports.Common;

public record GetReportByIdResponse(
    Guid Id,
    string LastStatus,
    string TrackingNumber,
    int CategoryId,
    string CategoryTitle,
    DateTime Sent,
    DateTime Deadline,
    DateTime? ResponseDeadline,
    string CitizenId,
    AddressResponse Address,
    string Comments,
    int? Rating,
    Visibility Visibility,
    bool IsIdentityVisible,
    int Likes,
    int CommentsCount,
    IEnumerable<Media> Medias
    )
{
    public static GetReportByIdResponse FromReport(Report report)
    {
        return new GetReportByIdResponse(
            report.Id,
            report.LastStatus,
            report.TrackingNumber,
            report.CategoryId,
            report.Category.Title,
            report.Sent,
            report.Deadline,
            report.ResponseDeadline,
            report.CitizenId,
            new AddressResponse(report.Address.Detail, report.Address.Location?.Y, report.Address.Location?.X, report.Address.RegionId, report.Address.Region?.Name),
            report.Comments,
            report.Rating,
            report.Visibility,
            report.IsIdentityVisible,
            report.Likes,
            report.CommentsCount,
            report.Medias);
    }

    public static Expression<Func<Report, GetReportByIdResponse>> GetSelector()
    {
        Expression<Func<Report, GetReportByIdResponse>> selector
            = report => new GetReportByIdResponse(
            report.Id,
            report.LastStatus,
            report.TrackingNumber,
            report.CategoryId,
            report.Category.Title,
            report.Sent,
            report.Deadline,
            report.ResponseDeadline,
            report.CitizenId,
            new AddressResponse(
                report.Address.Detail,
                report.Address.Location == null ? null : report.Address.Location.Y,
                report.Address.Location == null ? null : report.Address.Location.X,
                report.Address.RegionId,
                report.Address.Region == null ? null : report.Address.Region.Name),
            report.Comments,
            report.Rating,
            report.Visibility,
            report.IsIdentityVisible,
            report.Likes,
            report.CommentsCount,
            report.Medias);
        return selector;
    }
};
