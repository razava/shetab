using Application.Common.FilterModels;
using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using MediatR;

namespace Application.Reports.Queries.GetReports;

public sealed record GetReportsQuery(
    PagingInfo PagingInfo,
    string UserId,
    List<string> Roles,
    string? FromRoleId,
    int InstanceId,
    FilterGetReportsModel? FilterGetReports = default!) : IRequest<Result<PagedList<GetReportsResponse>>>;

public record GetReportsResponse(
    Guid Id,
    string LastStatus,
    string TrackingNumber,
    int CategoryId,
    string CategoryTitle,
    DateTime Sent,
    DateTime Deadline,
    DateTime? ResponseDeadline,
    int? Rating
    );