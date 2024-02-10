using Domain.Models.Relational;

namespace Application.Categories.Queries.GetStaffCategories;

public record GetStaffCategoriesQuery(
    int InstanceId,
    string UserId) : IRequest<Result<Category>>;

