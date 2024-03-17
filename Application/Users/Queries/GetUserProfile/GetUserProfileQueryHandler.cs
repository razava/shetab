using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.IdentityAggregate;

namespace Application.Users.Queries.GetUserProfile;

internal class GetUserProfileQueryHandler(IUserRepository userRepository) : IRequestHandler<GetUserProfileQuery, Result<ApplicationUser>>
{

    public async Task<Result<ApplicationUser>> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
    {
        var result = await userRepository.GetSingleAsync(u => u.Id == request.UserId);
        if (result == null)
        {
            return NotFoundErrors.User;
        }

        return result;
    }
}
