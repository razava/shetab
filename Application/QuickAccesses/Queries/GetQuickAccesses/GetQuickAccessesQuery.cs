using Application.Common.FilterModels;
using Application.QuickAccesses.Common;

namespace Application.QuickAccesses.Queries.GetQuickAccesses;

public record GetQuickAccessesQuery(
    int InstanceId,
    QueryFilterModel? FilterModel = default!) : IRequest<Result<List<AdminGetQuickAccessResponse>>>;
