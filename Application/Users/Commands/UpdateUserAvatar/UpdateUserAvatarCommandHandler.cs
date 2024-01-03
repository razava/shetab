using Application.Common.Exceptions;
using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.Common;
using Infrastructure.Storage;
using MediatR;

namespace Application.Users.Commands.UpdateUserAvatar;

internal class UpdateUserAvatarCommandHandler : IRequestHandler<UpdateUserAvatarCommand, Media>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStorageService _storageService;
    public UpdateUserAvatarCommandHandler(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IStorageService storageService)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _storageService = storageService;
    }

    public async Task<Media> Handle(UpdateUserAvatarCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetSingleAsync(u => u.Id == request.UserId);
        if (user is null)
            throw new NotFoundException("کاربر");
        var avatar = await _storageService.WriteFileAsync(request.Avatar, AttachmentType.Avatar);
        if (avatar is null)
            throw new SaveImageFailedException();
        user.Avatar = avatar;
        _userRepository.Update(user);
        await _unitOfWork.SaveAsync();

        return avatar;
    }
}
