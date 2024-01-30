using MediatR;

namespace Application.Workspaces.Queries.GetPossibleSources;

public sealed record GetPossibleSourcesQuery(
    string UserId,
    List<string> RoleNames) : IRequest<Result<List<PossibleSourceResponse>>>;
