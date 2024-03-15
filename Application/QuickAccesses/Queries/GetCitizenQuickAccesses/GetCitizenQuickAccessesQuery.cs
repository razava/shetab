using Application.Common.FilterModels;
using Domain.Models.Relational;

namespace Application.QuickAccesses.Queries.GetCitizenQuickAccesses;

public record GetCitizenQuickAccessesQuery(
    int InstanceId,
    List<string> Roles,
    QueryFilterModel? FilterModel = default!,
    bool ReturnAll = false) : IRequest<Result<List<QuickAccess>>>;
