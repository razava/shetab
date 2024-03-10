using Application.Info.Common;

namespace Application.Info.Queries.GetReportFilters;

public record GetReportFiltersQuery(int? InstanceId, string UserId, List<string> UserRoles) : IRequest<Result<ReportFiltersResponse>>;
public record ReportFiltersResponse(
    List<FilterItem<int>>? Regions,
    FilterCategory? Category,
    List<FilterItem<int>>? States,
    List<FilterItem<int>>? Priorities,
    List<FilterItem<int>>? ReportsToInclude,
    List<FilterItem<int>>? SatisfactionValues,
    List<FilterItem<string>>? Executives);
