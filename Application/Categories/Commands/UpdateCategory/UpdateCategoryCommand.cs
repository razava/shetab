using Domain.Models.Relational;
using MediatR;

namespace Application.Categories.Commands.UpdateCategory;

public sealed record UpdateCategoryCommand(
    Category Category) : IRequest<Category>;

