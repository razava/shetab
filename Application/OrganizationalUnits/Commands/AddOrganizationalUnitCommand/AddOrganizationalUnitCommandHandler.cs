using Application.Common.Exceptions;
using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using Domain.Models.Relational.Common;
using Domain.Models.Relational.IdentityAggregate;
using MediatR;

namespace Application.OrganizationalUnits.Commands.AddOrganizationalUnitCommand;
internal class AddOrganizationalUnitCommandHandler : IRequestHandler<AddOrganizationalUnitCommand, OrganizationalUnit>
{
    private readonly IUserRepository _userRepository;
    private readonly IOrganizationalUnitRepository _organizationalUnitRepository;
    private readonly IActorRepository _actorRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddOrganizationalUnitCommandHandler(
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

    public async Task<OrganizationalUnit> Handle(AddOrganizationalUnitCommand request, CancellationToken cancellationToken)
    {
        var user = new ApplicationUser() { UserName = request.Username, ShahrbinInstanceId = request.InstanceId };
        await _userRepository.CreateAsync(user, request.Password);

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

        _organizationalUnitRepository.Insert(result);
        await _unitOfWork.SaveAsync();

        return result;
    }
}