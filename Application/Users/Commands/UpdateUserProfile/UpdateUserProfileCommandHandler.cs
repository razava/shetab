using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.IdentityAggregate;
using MediatR;

namespace Application.Users.Commands.UpdateUserProfile;

internal class UpdateUserProfileCommandHandler : IRequestHandler<UpdateUserProfileCommand, ApplicationUser>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    public UpdateUserProfileCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ApplicationUser> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetSingleAsync(u => u.Id == request.UserId);
        if (user is null)
            throw new Exception("Not found.");
        user.FirstName = request.FirstName??user.FirstName;
        user.LastName = request.LastName ?? user.LastName;

        _userRepository.Update(user);
        await _unitOfWork.SaveAsync();

        return user;
    }
}
