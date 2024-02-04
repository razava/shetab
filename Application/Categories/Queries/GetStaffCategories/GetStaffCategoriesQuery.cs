using Application.Common.FilterModels;
using Domain.Models.Relational;
using MediatR;

namespace Application.Categories.Queries.GetStaffCategories;

public record GetStaffCategoriesQuery(
    int InstanceId,
    string UserId) : IRequest<Result<Category>>;

