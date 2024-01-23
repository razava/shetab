using Application.Common.Interfaces.Persistence;
using Application.Users.Common;
using Application.Users.Queries.GetRoles;
using Domain.Models.Relational.Common;
using Domain.Models.Relational.ProcessAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Queries.GetRegions;

internal class GetUserRegionsQueryHandler(IActorRepository actorRepository) : IRequestHandler<GetUserRegionsQuery, Result<List<IsInRegionModel>>>
{
    public async Task<Result<List<IsInRegionModel>>> Handle(GetUserRegionsQuery request, CancellationToken cancellationToken)
    {
        var result = await actorRepository.GetUserRegionsAsync(request.InstanceId, request.UserId);
        return result;
    }
}
