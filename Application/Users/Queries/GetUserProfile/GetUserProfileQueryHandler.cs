using Application.Common.Exceptions;
using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.IdentityAggregate;
using MediatR;

namespace Application.Users.Queries.GetUserProfile;

internal class GetUserProfileQueryHandler : IRequestHandler<GetUserProfileQuery, ApplicationUser>
{
    private readonly IUserRepository _userRepository;

    public GetUserProfileQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<ApplicationUser> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
    {
        var result = await _userRepository.GetSingleAsync(u => u.Id == request.UserId);
        if (result == null)
        {
            throw new NotFoundException("کاربر");
        }

        return result;
    }
}
