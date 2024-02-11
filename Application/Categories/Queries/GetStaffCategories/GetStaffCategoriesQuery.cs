using Application.Categories.Common;

namespace Application.Categories.Queries.GetStaffCategories;

public record GetStaffCategoriesQuery(
    int InstanceId,
    string UserId,
    List<string> Roles) : IRequest<Result<CategoryResponse>>;

