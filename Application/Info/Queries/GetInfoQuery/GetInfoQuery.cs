using Application.Common.Interfaces.Info;
using Application.Info.Common;
using System.ComponentModel;

namespace Application.Info.Queries.GetInfoQuery;

public record GetInfoQuery(
    int Code,
    int InstanceId,
    string UserId,
    List<string> Roles,
    string? Parameter,
    List<GeoPoint>? Geometry,
    List<ReportsToInclude>? ReportsToInclude,
    ReportFilters ReportFilters)
    : IRequest<Result<InfoModel>>;


public record ReportFilters(
    string? Query,
    List<int>? Regions,
    List<int>? Categories,
    List<int>? States,
    List<int>? Priorities,
    DateTime? FromDate,
    DateTime? ToDate,
    List<int>? SatisfactionValues,
    List<string>? Execitives);

public enum ReportsToInclude
{
    [Description("در انتظار بررسی")]
    Interacted,
    [Description("ارجاع داده شده")]
    InCartable
}