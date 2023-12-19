using Application.Common.Exceptions;
using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.IdentityAggregate;
using MediatR;

namespace Application.Users.Queries.GetUserById;

internal class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, ApplicationUser>
{
    private readonly IUserRepository _userRepository;

    public GetUserByIdQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<ApplicationUser> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetSingleAsync(u => u.Id == request.UserId);
        if (user is null)
            throw new NotFoundException("User");
        return user;
    }
}
