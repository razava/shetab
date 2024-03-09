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

public record FilterItem<T>(string Title, T Value);
public record FilterCategory(
    int Id,
    int Order,
    string Code,
    string Title,
    ICollection<FilterCategory> Categories);