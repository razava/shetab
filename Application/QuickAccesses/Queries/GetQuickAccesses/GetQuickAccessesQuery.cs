using Application.Common.FilterModels;
using Domain.Models.Relational;
using MediatR;

namespace Application.QuickAccesses.Queries.GetQuickAccesses;

public record GetQuickAccessesQuery(
    int InstanceId,
    QueryFilterModel? FilterModel = default!,
    bool ReturnAll = false) : IRequest<Result<List<QuickAccess>>>;
