using Application.Common.Interfaces.Persistence;

namespace Application.Users.Queries.GetUsers;

internal class GetUsersQueryHandler(IUserRepository userRepository) 
    : IRequestHandler<GetUsersQuery, Result<PagedList<GetUsersListResponse>>>
{
    public async Task<Result<PagedList<GetUsersListResponse>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {

        var result = await userRepository.GetUsersAsync(
            request.InstanceId,
            request.PagingInfo,
            request.UserFilters,
            GetUsersListResponse.GetSelector());

        return result;
    }
}


