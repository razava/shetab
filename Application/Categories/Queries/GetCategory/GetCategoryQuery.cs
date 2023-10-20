using Domain.Models.Relational;
using MediatR;

namespace Application.Categories.Queries.GetCategory;

public sealed record GetCategoryQuery() : IRequest<List<Category>>;

