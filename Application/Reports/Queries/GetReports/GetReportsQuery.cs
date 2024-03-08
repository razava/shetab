using Application.Common.FilterModels;
using Application.Common.Interfaces.Persistence;
using Application.Info.Queries.GetInfoQuery;
using Application.Reports.Common;

namespace Application.Reports.Queries.GetReports;

public sealed record GetReportsQuery(
    PagingInfo PagingInfo,
    string UserId,
    List<string> Roles,
    string? FromRoleId,
    int InstanceId,
    ReportFilters ReportFilters) : IRequest<Result<PagedList<GetReportsResponse>>>;
