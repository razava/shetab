using Domain.Models.Relational;
using MediatR;

namespace Application.Categories.Queries.GetCategoryById;

public sealed record GetCategoryByIdQuery(int Id) : IRequest<Category>;

