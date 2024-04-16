using Application.Common.Helper;
using Application.Common.Interfaces.Persistence;
using Microsoft.IdentityModel.Tokens;
using SharedKernel.Successes;

namespace Application.Users.Commands.UpdateUserProfile;

internal class UpdateUserProfileCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateUserProfileCommand, Result<bool>>
{
    
    public async Task<Result<bool>> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetSingleAsync(u => u.Id == request.UserId);
        if (user is null)
            return NotFoundErrors.User;
        if (user.PhoneNumber.IsNullOrEmpty()
            && request.TwoFactorEnabled is not null
            && request.TwoFactorEnabled.Value)
        {
            return AuthenticationErrors.InvalidPhoneNumber;
        }

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
        user.SmsAlert = request.SmsAlert ?? user.SmsAlert;

        userRepository.Update(user);
        await unitOfWork.SaveAsync();

        return ResultMethods.GetResult(true, UpdateSuccess.UserProfile);
    }
}
