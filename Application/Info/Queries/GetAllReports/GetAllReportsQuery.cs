using Application.Common.Interfaces.Info;
using Application.Common.Interfaces.Persistence;
using Application.Info.Queries.GetInfo;
using Application.Reports.Common;

namespace Application.Info.Queries.GetAllReports;

public sealed record GetAllReportsQuery(
    PagingInfo PagingInfo,
    int InstanceId,
    string UserId,
    List<string> Roles,
    List<GeoPoint>? Geometry,
    List<ReportsToInclude>? ReportsToInclude,
    ReportFilters ReportFilters)
    : IRequest<Result<PagedList<GetReportsResponse>>>;

