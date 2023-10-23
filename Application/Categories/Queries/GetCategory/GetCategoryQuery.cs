using Domain.Models.Relational;
using MediatR;

namespace Application.Categories.Queries.GetCategory;

public sealed record GetCategoryQuery(int InstanceId) : IRequest<List<Category>>;

