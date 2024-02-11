using Application.Categories.Common;

namespace Application.Categories.Queries.GetCategoryById;

public sealed record GetCategoryByIdQuery(int Id) : IRequest<Result<CategoryDetailResponse>>;

