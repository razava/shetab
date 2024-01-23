using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.IdentityAggregate;
using MediatR;

namespace Application.Users.Queries.GetUsers;

internal class GetUsersQueryHandler(IUserRepository userRepository) : IRequestHandler<GetUsersQuery, Result<PagedList<ApplicationUser>>>
{
    public async Task<Result<PagedList<ApplicationUser>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var result = await userRepository.GetPagedAsync(request.PagingInfo,
            u => u.ShahrbinInstanceId != null && u.ShahrbinInstanceId == request.InstanceId,
            false);
        return result;
    }
}
