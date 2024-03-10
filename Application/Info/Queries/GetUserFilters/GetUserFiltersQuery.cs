using Application.Info.Common;

namespace Application.Info.Queries.GetUserFilters;

public record GetUserFiltersQuery(int InstanceId) : IRequest<Result<UserFiltersResponse>>;
public record UserFiltersResponse(
    List<FilterItem<int>>? Regions,
    List<FilterItem<string>>? Roles);
