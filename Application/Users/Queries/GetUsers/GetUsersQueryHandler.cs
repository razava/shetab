using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.IdentityAggregate;
using MediatR;

namespace Application.Users.Queries.GetUsers;

internal class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, PagedList<ApplicationUser>>
{
    private readonly IUserRepository _userRepository;

    public GetUsersQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<PagedList<ApplicationUser>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var result = await _userRepository.GetPagedAsync(request.PagingInfo,
            u => u.ShahrbinInstanceId != null && u.ShahrbinInstanceId == request.InstanceId,
            false);
        return result;
    }
}
