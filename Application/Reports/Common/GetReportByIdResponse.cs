﻿using Application.Common.Interfaces.Persistence;
using Application.Forms.Common;
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
    DateTime? Finished,
    DateTime? Responsed,
    string CitizenId,
    AddressResponse Address,
    string Comments,
    int? Rating,
    Visibility Visibility,
    Priority Priority,
    bool IsIdentityVisible,
    int Likes,
    bool IsFeedbacked,
    int CommentsCount,
    IEnumerable<Media> Medias,
    FormResponse? Form,
    int? CurrentActorId
    )
{
    public ActorIdentityResponse? CurrentActor { get; set; }
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
            report.Finished,
            report.Responsed,
            report.CitizenId,
            new AddressResponse(report.Address.Detail, report.Address.Location?.Y, report.Address.Location?.X, report.Address.RegionId, report.Address.Region?.Name),
            report.Comments,
            report.Rating,
            report.Visibility,
            report.Priority,
            report.IsIdentityVisible,
            report.Likes,
            report.IsFeedbacked,
            report.CommentsCount,
            report.Medias,
            FormResponse.FromForm(report.Category.Form),
            report.CurrentActorId);
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
            report.Finished,
            report.Responsed,
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
            report.Priority,
            report.IsIdentityVisible,
            report.Likes,
            report.IsFeedbacked,
            report.CommentsCount,
            report.Medias,
            FormResponse.FromForm(report.Category.Form),
            report.CurrentActorId);
        return selector;
    }
};
