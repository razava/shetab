using Application.Common.Helper;
using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using Domain.Models.Relational.Common;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Successes;

namespace Application.OrganizationalUnits.Commands.UpdateOrganizationalUnit;
internal class UpdateOrganizationalUnitCommandHandler(
    IUserRepository userRepository,
    IOrganizationalUnitRepository organizationalUnitRepository,
    IActorRepository actorRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateOrganizationalUnitCommand, Result<OrganizationalUnit>>
{

    public async Task<Result<OrganizationalUnit>> Handle(UpdateOrganizationalUnitCommand request, CancellationToken cancellationToken)
    {
        //TODO: CHECK AND REVISE ORGANIZATIONAL UNIT COMMANDS ASAP!
        var organizationalUnit = await organizationalUnitRepository.GetSingleAsync(ou => ou.Id == request.OrganizationalUnitId, true, "OrganizationalUnits");
        if (organizationalUnit is null)
        {
            return NotFoundErrors.OrganizationalUnit;
        }

        //Check for cycle
        var ous = await organizationalUnitRepository
            .GetAsync(p => request.OrganizationalUnitsIds.Contains(p.Id));

        var flattenOus = new List<OrganizationalUnit>(ous);
        var queue = new Queue<OrganizationalUnit>(ous);

        while (queue.Count > 0)
        {
            var t = queue.Dequeue();
            flattenOus.Add(t);
            var childOus = await unitOfWork.DbContext.Set<OrganizationalUnit>()
                .Where(p => p.Id == t.Id)
                .Select(p => p.OrganizationalUnits.Where(q => q.Type == OrganizationalUnitType.OrganizationalUnit))
                .AsSingleQuery()
                .SingleOrDefaultAsync();
            if (childOus is null)
                continue;
            foreach (var child in childOus)
            {
                queue.Enqueue(child);
            }
        }
        if (flattenOus.Any(p => p.Id == organizationalUnit.Id))
        {
            return OperationErrors.LoopMade;
        }



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
                ShahrbinInstanceId = organizationalUnit.ShahrbinInstanceId,
                ActorId = executiveActor.Id,
                Type = OrganizationalUnitType.Executive,
                UserId = executiveActor.Identifier,
                Title = executiveUser.Title,
            });
        }

        organizationalUnit.OrganizationalUnits.Clear();
        organizationalUnit.OrganizationalUnits.AddRange(newOus);
        organizationalUnit.OrganizationalUnits.AddRange(existingExecutiveOUs);
        organizationalUnit.OrganizationalUnits.AddRange(containedOus);
        organizationalUnit.Title = request.Title;

        organizationalUnitRepository.Update(organizationalUnit);
        await unitOfWork.SaveAsync();

        return ResultMethods.GetResult(organizationalUnit, UpdateSuccess.OrganizationalUnit);
    }
}