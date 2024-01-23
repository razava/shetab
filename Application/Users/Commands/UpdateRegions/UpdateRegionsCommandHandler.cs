using Application.Common.Interfaces.Persistence;
using Application.Users.Commands.UpdateRoles;
using MediatR;

namespace Application.Users.Commands.UpdateRegions;

internal class UpdateRegionsCommandHandler(IUnitOfWork unitOfWork, IActorRepository actorRepository) : IRequestHandler<UpdateRegionsCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(UpdateRegionsCommand request, CancellationToken cancellationToken)
    {
        var result = await actorRepository.UpdateUserRegionsAsync(request.InstanceId, request.UserId, request.Regions);
        await unitOfWork.SaveAsync();
        return result;
    }
}
