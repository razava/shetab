namespace Application.Reports.Queries.GetReportFilters;

public record GetReportFiltersQuery(int? InstanceId, string UserId, List<string> UserRoles) : IRequest<Result<ReportFiltersResponse>>;
public record ReportFiltersResponse(
    List<FilterItem>? Regions,
    FilterCategory? Category,
    List<FilterItem>? States,
    List<FilterItem>? Priorities,
    List<FilterItem>? ReportsToInclude,
    List<FilterItem>? MinSatisfaction,
    List<FilterItem>? MaxSatisfaction);

public record FilterItem(string Title, int Value);
public record FilterCategory(
    int Id,
    int Order,
    string Code,
    string Title,
    ICollection<FilterCategory> Categories);