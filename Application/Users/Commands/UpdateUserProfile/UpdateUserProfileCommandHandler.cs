using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.IdentityAggregate;

namespace Application.Users.Commands.UpdateUserProfile;

internal class UpdateUserProfileCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateUserProfileCommand, Result<ApplicationUser>>
{
    
    public async Task<Result<ApplicationUser>> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetSingleAsync(u => u.Id == request.UserId);
        if (user is null)
            return NotFoundErrors.User;
        user.FirstName = request.FirstName ?? user.FirstName;
        user.LastName = request.LastName ?? user.LastName;
        user.Title = request.Title ?? user.Title;
        user.Organization = request.Organization ?? user.Organization;
        user.NationalId = request.NationalId ?? user.NationalId;
        user.TwoFactorEnabled = request.TwoFactorEnabled ?? user.TwoFactorEnabled;
        user.Gender = request.Gender ?? user.Gender;
        user.Education = request.Education ?? user.Education;
        user.BirthDate = request.BirthDate ?? user.BirthDate;
        user.PhoneNumber2 = request.PhoneNumber2 ?? user.PhoneNumber2;

        userRepository.Update(user);
        await unitOfWork.SaveAsync();

        return user;
    }
}
