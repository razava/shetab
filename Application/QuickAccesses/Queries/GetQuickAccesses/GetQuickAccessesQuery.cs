using Application.Common.FilterModels;
using Domain.Models.Relational;

namespace Application.QuickAccesses.Queries.GetQuickAccesses;

public record GetQuickAccessesQuery(
    int InstanceId,
    QueryFilterModel? FilterModel = default!) : IRequest<Result<List<QuickAccess>>>;
