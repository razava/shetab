using Application.Common.Interfaces.Persistence;

namespace Application.Users.Queries.GetUserProfile;

internal class GetUserProfileQueryHandler(IUserRepository userRepository) : IRequestHandler<GetUserProfileQuery, Result<GetProfileResponse>>
{

    public async Task<Result<GetProfileResponse>> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
    {
        var result = await userRepository.GetUserById(
            request.UserId,
            GetProfileResponse.GetSelector());

        if (result == null)
            return NotFoundErrors.User;

        return result;
    }
}
