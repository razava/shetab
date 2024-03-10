using Application.Common.Interfaces.Persistence;
using Application.Users.Common;

namespace Application.Users.Queries.GetUserRegions;

internal class GetUserRegionsQueryHandler(IActorRepository actorRepository) : IRequestHandler<GetUserRegionsQuery, Result<List<IsInRegionModel>>>
{
    public async Task<Result<List<IsInRegionModel>>> Handle(GetUserRegionsQuery request, CancellationToken cancellationToken)
    {
        var result = await actorRepository.GetUserRegionsAsync(request.InstanceId, request.UserId);
        return result;
    }
}
