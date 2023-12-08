using Application.Common.Interfaces.Persistence;
using Application.Users.Commands.UpdateRoles;
using MediatR;

namespace Application.Users.Commands.UpdateRegions;

internal class UpdateRegionsCommandHandler : IRequestHandler<UpdateRegionsCommand, bool>
{
    IActorRepository _actorRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateRegionsCommandHandler(IUnitOfWork unitOfWork, IActorRepository actorRepository)
    {
        _unitOfWork = unitOfWork;
        _actorRepository = actorRepository;
    }

    public async Task<bool> Handle(UpdateRegionsCommand request, CancellationToken cancellationToken)
    {
        var result = await _actorRepository.UpdateUserRegionsAsync(request.InstanceId, request.UserId, request.Regions);
        await _unitOfWork.SaveAsync();
        return result;
    }
}
