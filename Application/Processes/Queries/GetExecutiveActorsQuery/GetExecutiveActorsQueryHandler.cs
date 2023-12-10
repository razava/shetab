using Application.Common.Interfaces.Persistence;
using Application.Common.Statics;
using MediatR;

namespace Application.Processes.Queries.GetExecutiveActorsQuery;

internal class GetExecutiveActorsQueryHandler : IRequestHandler<GetExecutiveActorsQuery, List<GetExecutiveActorsResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly IActorRepository _actorRepository;

    public GetExecutiveActorsQueryHandler(IUserRepository userRepository, IActorRepository actorRepository)
    {
        _userRepository = userRepository;
        _actorRepository = actorRepository;
    }

    public async Task<List<GetExecutiveActorsResponse>> Handle(GetExecutiveActorsQuery request, CancellationToken cancellationToken)
    {
        var executives = await _userRepository.GetUsersInRole(RoleNames.Executive);
        var executivesIds = executives.Select(executives => executives.Id);
        var executiveActors = (await _actorRepository.GetAsync(a => executivesIds.Contains(a.Identifier), false)).ToList();

        var result = new List<GetExecutiveActorsResponse>();
        foreach(var actor in executiveActors)
        {
            var user = executives.Where(e => e.Id == actor.Identifier).Single();
            result.Add(new GetExecutiveActorsResponse(actor.Id, user.FirstName, user.LastName, user.Title));
        }

        return result;
    }
}
