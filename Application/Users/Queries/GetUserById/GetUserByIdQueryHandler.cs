using Application.Common.Interfaces.Persistence;

namespace Application.Users.Queries.GetUserById;

internal class GetUserByIdQueryHandler(IUserRepository userRepository) 
    : IRequestHandler<GetUserByIdQuery, Result<AdminGetUserDetailsResponse>>
{

    public async Task<Result<AdminGetUserDetailsResponse>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await userRepository.GetUserById(
            request.UserId,
            AdminGetUserDetailsResponse.GetSelector());

        if (result is null)
            return NotFoundErrors.User;
        return result;
    }
}
