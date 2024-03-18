using Application.Common.Interfaces.Persistence;
using Application.Common.Statics;
using Application.Processes.Common;

namespace Application.Processes.Queries.GetExecutiveActors;

internal class GetExecutiveActorsQueryHandler(IUserRepository userRepository, IActorRepository actorRepository) : IRequestHandler<GetExecutiveActorsQuery, Result<List<GetExecutiveActorsResponse>>>
{

    public async Task<Result<List<GetExecutiveActorsResponse>>> Handle(GetExecutiveActorsQuery request, CancellationToken cancellationToken)
    {
        //TODO: Implement this in repository
        var executives = (await userRepository.GetUsersInRole(RoleNames.Executive)).Where(u => u.ShahrbinInstanceId == request.InstanceId).ToList();
        var executivesIds = executives.Select(executives => executives.Id);
        var executiveActors = (await actorRepository.GetAsync(a => executivesIds.Contains(a.Identifier), false)).ToList();

        var result = new List<GetExecutiveActorsResponse>();
        foreach (var actor in executiveActors)
        {
            var user = executives.Where(e => e.Id == actor.Identifier).Single();
            var displayName = $"{user.Title} ({user.FirstName} {user.LastName})".Replace("()", "");
            result.Add(new GetExecutiveActorsResponse(actor.Id, user.FirstName, user.LastName, user.Title, displayName));
        }

        return result;
    }
}
