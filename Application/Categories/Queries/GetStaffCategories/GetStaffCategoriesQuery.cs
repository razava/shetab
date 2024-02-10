using Application.Categories.Queries.GetCategory;

namespace Application.Categories.Queries.GetStaffCategories;

public record GetStaffCategoriesQuery(
    int InstanceId,
    string UserId,
    List<string> Roles) : IRequest<Result<CategoryResponse>>;

