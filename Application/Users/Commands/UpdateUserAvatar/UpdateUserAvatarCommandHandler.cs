using Application.Common.Helper;
using Application.Common.Interfaces.Persistence;
using Infrastructure.Storage;
using SharedKernel.Successes;

namespace Application.Users.Commands.UpdateUserAvatar;

internal class UpdateUserAvatarCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    IStorageService storageService) : IRequestHandler<UpdateUserAvatarCommand, Result<bool>>
{
    
    public async Task<Result<bool>> Handle(UpdateUserAvatarCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetSingleAsync(u => u.Id == request.UserId);
        if (user is null)
            return NotFoundErrors.User;
        var avatar = await storageService.WriteFileAsync(request.Avatar, AttachmentType.Avatar);
        if (avatar is null)
            return AttachmentErrors.SaveImageFailed;
        user.Avatar = avatar;
        userRepository.Update(user);
        await unitOfWork.SaveAsync();

        return ResultMethods.GetResult(true, UpdateSuccess.Avatar);
    }
}
