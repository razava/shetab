using Application.Common.Interfaces.Persistence;

namespace Application.Reports.Queries.GetPossibleTransitions;

public sealed record GetPossibleTransitionsQuery(
    Guid reportId,
    string userId,
    int instanceId) : IRequest<Result<List<PossibleTransitionResponse>>>;

