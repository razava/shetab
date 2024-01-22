using Application.Common.FilterModels;
using Domain.Models.Relational;
using MediatR;

namespace Application.Categories.Queries.GetCategory;

public sealed record GetCategoryQuery(
    int InstanceId,
    QueryFilterModel? FilterModel = default!,
    bool ReturnAll = false) : IRequest<Result<List<Category>>>;

