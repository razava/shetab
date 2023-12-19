using Application.Common.Exceptions;
using Application.Common.Interfaces.Persistence;
using Application.OrganizationalUnits.Commands.AddOrganizationalUnitCommand;
using Domain.Models.Relational;
using Domain.Models.Relational.Common;
using Domain.Models.Relational.IdentityAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.OrganizationalUnits.Commands.UpdateOrganizationalUnitCommand;
internal class UpdateOrganizationalUnitCommandHandler : IRequestHandler<UpdateOrganizationalUnitCommand, OrganizationalUnit>
{
    private readonly IUserRepository _userRepository;
    private readonly IOrganizationalUnitRepository _organizationalUnitRepository;
    private readonly IActorRepository _actorRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateOrganizationalUnitCommandHandler(
        IUserRepository userRepository,
        IOrganizationalUnitRepository organizationalUnitRepository,
        IActorRepository actorRepository,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _organizationalUnitRepository = organizationalUnitRepository;
        _actorRepository = actorRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<OrganizationalUnit> Handle(UpdateOrganizationalUnitCommand request, CancellationToken cancellationToken)
    {
        //TODO: CHECK AND REVISE ORGANIZATIONAL UNIT COMMANDS ASAP!
        var organizationalUnit = await _organizationalUnitRepository.GetSingleAsync(ou => ou.Id == request.OrganizationalUnitId);
        if(organizationalUnit is null)
        {
            throw new NotFoundException("Organizational Unit");
        }

        //Check for cycle
        var ous = await _organizationalUnitRepository
            .GetAsync(p => request.OrganizationalUnitsIds.Contains(p.Id));

        var flattenOus = new List<OrganizationalUnit>(ous);
        var queue = new Queue<OrganizationalUnit>(ous);

        while (queue.Count > 0)
        {
            var t = queue.Dequeue();
            flattenOus.Add(t);
            var childOus = await _unitOfWork.DbContext.Set<OrganizationalUnit>()
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
            throw new LoopMadeException("حلقه ای در ساختار سازمانی ایجاد شده است.");
        }



        var containedOus = await _organizationalUnitRepository.GetAsync(ou => request.OrganizationalUnitsIds.Contains(ou.Id));

        var executiveActors = await _actorRepository.GetAsync(a => request.ExecutiveActorsIds.Contains(a.Id));
        var existingExecutiveOUs = await _organizationalUnitRepository.GetAsync(ou => ou.ActorId != null
            && request.ExecutiveActorsIds.Contains(ou.ActorId.Value));

        var executiveActoresNotOuAlready = executiveActors
            .Where(ea => !existingExecutiveOUs.Any(eou => eou.ActorId == ea.Id)).ToList();
        var executiveIdentifiers = executiveActoresNotOuAlready.Select(ea => ea.Identifier).ToList();
        var executiveUsers = await _userRepository.GetAsync(u => executiveIdentifiers.Contains(u.Id));
        var newOus = new List<OrganizationalUnit>();
        foreach (var executiveActor in executiveActoresNotOuAlready)
        {
            var executiveUser = executiveUsers.SingleOrDefault(eu => eu.Id == executiveActor.Identifier);
            if (executiveUser is null)
                throw new NotFoundException("ExecutiveUser");
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

        _organizationalUnitRepository.Update(organizationalUnit);
        await _unitOfWork.SaveAsync();

        return organizationalUnit;
    }
}