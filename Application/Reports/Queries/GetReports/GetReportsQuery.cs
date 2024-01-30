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
    FilterGetReportsModel? FilterGetReports = default!) : IRequest<Result<PagedList<Report>>>;

