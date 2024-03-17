using Application.Common.Interfaces.Info;
using Application.Info.Queries.GetInfo;

namespace Application.Info.Queries.GetExcel;

public sealed record GetExcelQuery(
    int InstanceId,
    string UserId,
    List<string> Roles,
    List<GeoPoint>? Geometry,
    List<ReportsToInclude>? ReportsToInclude,
    ReportFilters ReportFilters)
    : IRequest<Result<MemoryStream>>;

