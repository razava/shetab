﻿using Domain.Models.Relational;
using Domain.Models.Relational.Common;
using System.Linq.Expressions;

namespace Application.Reports.Common;

public record GetReportsResponse(
    Guid Id,
    ReportState ReportState,
    string LastStatus,
    string TrackingNumber,
    int CategoryId,
    string CategoryTitle,
    DateTime Sent,
    DateTime Deadline,
    DateTime? ResponseDeadline,
    int? Rating
    )
{
    public static GetReportsResponse FromReport(Report report)
    {
        return new GetReportsResponse(
                report.Id,
                report.ReportState,
                report.LastStatus,
                report.TrackingNumber,
                report.CategoryId,
                report.Category.Title,
                report.Sent,
                report.Deadline,
                report.ResponseDeadline,
                report.Rating);
    }

    public static Expression<Func<Report, GetReportsResponse>> GetSelector()
    {
        Expression<Func<Report, GetReportsResponse>> selector
            = report => new GetReportsResponse(
                report.Id,
                report.ReportState,
                report.LastStatus,
                report.TrackingNumber,
                report.CategoryId,
                report.Category.Title,
                report.Sent,
                report.Deadline,
                report.ResponseDeadline,
                report.Rating);
        return selector;
    }

}
