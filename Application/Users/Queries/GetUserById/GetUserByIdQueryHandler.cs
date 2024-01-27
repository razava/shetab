using Application.Common.Exceptions;
using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.IdentityAggregate;
using MediatR;

namespace Application.Users.Queries.GetUserById;

internal class GetUserByIdQueryHandler(IUserRepository userRepository) : IRequestHandler<GetUserByIdQuery, Result<ApplicationUser>>
{

    public async Task<Result<ApplicationUser>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetSingleAsync(u => u.Id == request.UserId);
        if (user is null)
            return NotFoundErrors.User;
        return user;
    }
}
