using Application.Common.Interfaces.Persistence;
using Application.Users.Common;
using Application.Users.Queries.GetRoles;
using Domain.Models.Relational.Common;
using Domain.Models.Relational.ProcessAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Queries.GetRegions;

internal class GetUserRegionsQueryHandler : IRequestHandler<GetUserRegionsQuery, List<IsInRegionModel>>
{
    private readonly IActorRepository _actorRepository;

    public GetUserRegionsQueryHandler(IActorRepository actorRepository)
    {
        _actorRepository = actorRepository;
    }

    public async Task<List<IsInRegionModel>> Handle(GetUserRegionsQuery request, CancellationToken cancellationToken)
    {
        var result = await _actorRepository.GetUserRegionsAsync(request.InstanceId, request.UserId);
        return result;
    }
}
