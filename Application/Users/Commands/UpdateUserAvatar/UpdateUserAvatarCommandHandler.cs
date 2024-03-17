using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.Common;
using Infrastructure.Storage;

namespace Application.Users.Commands.UpdateUserAvatar;

internal class UpdateUserAvatarCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    IStorageService storageService) : IRequestHandler<UpdateUserAvatarCommand, Result<Media>>
{
    
    public async Task<Result<Media>> Handle(UpdateUserAvatarCommand request, CancellationToken cancellationToken)
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

        return avatar;
    }
}
