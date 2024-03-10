using Application.Common.Interfaces.Persistence;
using Application.Common.Statics;
using Domain.Models.Relational.IdentityAggregate;

namespace Application.Setup.Commands.AddGoldenUser;

internal class AddGoldenUserCommandHandler(IUserRepository userRepository)
    : IRequestHandler<AddGoldenUserCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(AddGoldenUserCommand request, CancellationToken cancellationToken)
    {
        var roleResult = await userRepository.CreateRoleAsync(new ApplicationRole() { Name = RoleNames.GoldenUser, Title = "کاربر طلایی" });

        var goldenUser = new ApplicationUser() { UserName = request.UserName, ShahrbinInstanceId = request.InstanceId };
        var userResult = await userRepository.CreateAsync(goldenUser, request.Password);
        if(userResult.Succeeded)
        {
            var addToRoleResult = await userRepository.AddToRoleAsync(goldenUser, RoleNames.GoldenUser);
            if (addToRoleResult.Succeeded)
            {
                return true;
            }
        }

        return false;
    }
}
