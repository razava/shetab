﻿using Application.Common.Helper;
using Application.Common.Interfaces.Persistence;
using Application.OrganizationalUnits.Common;
using Domain.Models.Relational;
using Domain.Models.Relational.Common;
using Domain.Models.Relational.IdentityAggregate;
using Domain.Models.Relational.ProcessAggregate;
using Mapster;
using SharedKernel.Successes;

namespace Application.OrganizationalUnits.Commands.AddOrganizationalUnit;
internal class AddOrganizationalUnitCommandHandler(
    IUserRepository userRepository,
    IOrganizationalUnitRepository organizationalUnitRepository,
    IActorRepository actorRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<AddOrganizationalUnitCommand, Result<GetOrganizationalUnitResponse>>
{
    public async Task<Result<GetOrganizationalUnitResponse>> Handle(AddOrganizationalUnitCommand request, CancellationToken cancellationToken)
    {
        var user = new ApplicationUser()
        {
            UserName = request.Username,
            ShahrbinInstanceId = request.InstanceId,
            Title = request.Title,
            PhoneNumberConfirmed = true
        };

        await userRepository.RegisterWithRoleAsync(user, request.Password, "Manager");

        var actor = new Actor()
        {
            Identifier = user.Id,
            Type = ActorType.Person
        };

        unitOfWork.DbContext.Set<Actor>().Add(actor);

        var containedOus = await organizationalUnitRepository.GetAsync(ou => request.OrganizationalUnitsIds.Contains(ou.Id));

        var executiveActors = await actorRepository.GetAsync(a => request.ExecutiveActorsIds.Contains(a.Id));
        var existingExecutiveOUs = await organizationalUnitRepository.GetAsync(ou => ou.ActorId != null
            && request.ExecutiveActorsIds.Contains(ou.ActorId.Value));

        var executiveActoresNotOuAlready = executiveActors
            .Where(ea => !existingExecutiveOUs.Any(eou => eou.ActorId == ea.Id)).ToList();
        var executiveIdentifiers = executiveActoresNotOuAlready.Select(ea => ea.Identifier).ToList();
        var executiveUsers = await userRepository.GetAsync(u => executiveIdentifiers.Contains(u.Id));
        var newOus = new List<OrganizationalUnit>();
        foreach (var executiveActor in executiveActoresNotOuAlready)
        {
            var executiveUser = executiveUsers.SingleOrDefault(eu => eu.Id == executiveActor.Identifier);
            if (executiveUser is null)
                return ServerNotFoundErrors.ExecutiveUser;
            newOus.Add(new OrganizationalUnit()
            {
                ShahrbinInstanceId = request.InstanceId,
                ActorId = executiveActor.Id,
                Type = OrganizationalUnitType.Executive,
                UserId = executiveActor.Identifier,
                Title = executiveUser.Title,
            });
        }

        var result = new OrganizationalUnit()
        {
            ShahrbinInstanceId = request.InstanceId,
            Type = OrganizationalUnitType.OrganizationalUnit,
            UserId = user.Id,
            Title = request.Title,
            OrganizationalUnits = new List<OrganizationalUnit>()
        };
        result.OrganizationalUnits.AddRange(newOus);
        result.OrganizationalUnits.AddRange(existingExecutiveOUs);
        result.OrganizationalUnits.AddRange(containedOus);

        organizationalUnitRepository.Insert(result);
        await unitOfWork.SaveAsync();

        return ResultMethods.GetResult(result.Adapt<GetOrganizationalUnitResponse>(), CreationSuccess.OrganizationalUnit);
    }
}