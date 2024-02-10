using Application.Common.FilterModels;
using Application.Common.Interfaces.Persistence;
using Application.Reports.Common;

namespace Application.Reports.Queries.GetReports;

public sealed record GetReportsQuery(
    PagingInfo PagingInfo,
    string UserId,
    List<string> Roles,
    string? FromRoleId,
    int InstanceId,
    FilterGetReportsModel? FilterGetReports = default!) : IRequest<Result<PagedList<GetReportsResponse>>>;
