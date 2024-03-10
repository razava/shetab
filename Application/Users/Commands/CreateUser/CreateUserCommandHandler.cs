using Application.Common.Interfaces.Persistence;
using Application.Common.Statics;
using Domain.Models.Relational.IdentityAggregate;
using Microsoft.IdentityModel.Tokens;

namespace Application.Users.Commands.CreateUser;

internal class CreateUserCommandHandler(IUserRepository userRepository) : IRequestHandler<CreateUserCommand, Result<ApplicationUser>>
{

    public async Task<Result<ApplicationUser>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        ApplicationUser user = new()
        {
            ShahrbinInstanceId = request.InstanceId,
            UserName = request.Username,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Title = request.Title,
            PhoneNumberConfirmed = true
        };
        await userRepository.CreateAsync(user, request.Password);

        if (request.Roles is not null)
        {
            request.Roles.RemoveAll(r => r == RoleNames.PowerUser || r == RoleNames.GoldenUser);
            await userRepository.AddToRolesAsync(user, request.Roles.ToArray());
        }

        return user;
    }
}
