using Application.Categories.Common;

namespace Application.Categories.Queries.GetCategory;

public sealed record GetCategoryQuery(
    int InstanceId,
    List<string>? Roles,
    bool ReturnAll = false) : IRequest<Result<CategoryResponse>>;

