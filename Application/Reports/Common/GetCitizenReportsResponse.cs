﻿using Azure.Core;
using Domain.Models.Relational;
using Domain.Models.Relational.Common;

namespace Application.Reports.Common;

public record GetCitizenReportsResponse(
    Guid Id,
    string LastStatus,
    string TrackingNumber,
    int CategoryId,
    string CategoryTitle,
    DateTime Sent,
    UserShortResponse Citizen,
    string Comments,
    string AddressDetail,
    bool IsLiked,
    int Likes,
    int CommentsCount,
    IEnumerable<Media> Medias)
{
    public static GetCitizenReportsResponse FromReport(Report report, string userId)
    {
        return new GetCitizenReportsResponse(
                report.Id,
                report.LastStatus,
                report.TrackingNumber,
                report.CategoryId,
                report.Category.Title,
                report.Sent,
                new UserShortResponse(report.Citizen.FirstName, report.Citizen.LastName, report.Citizen.Avatar),
                report.Comments,
                report.Address.Detail,
                report.LikedBy.Any(r => r.Id == userId),
                report.Likes,
                report.CommentsCount,
                report.Medias);
    }
}