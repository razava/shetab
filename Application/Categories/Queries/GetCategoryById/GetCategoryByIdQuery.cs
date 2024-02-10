using Domain.Models.Relational;

namespace Application.Categories.Queries.GetCategoryById;

public sealed record GetCategoryByIdQuery(int Id) : IRequest<Result<Category>>;

