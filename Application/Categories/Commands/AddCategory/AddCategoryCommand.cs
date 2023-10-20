using Domain.Models.Relational;
using MediatR;

namespace Application.Categories.Commands.AddCategory;

public sealed record AddCategoryCommand(
    Category Category) : IRequest<Category>;

