using Application.Common.Helper;
using Application.Common.Interfaces.Persistence;
using SharedKernel.Successes;

namespace Application.Users.Commands.UpdateRoles;

internal class UpdateRolesCommandHandler(IUserRepository userRepository) : IRequestHandler<UpdateRolesCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(UpdateRolesCommand request, CancellationToken cancellationToken)
    {
        var roles = request.Roles.Where(role => role.IsIn).Select(role => role.RoleName)
            .ToList();
        if (roles is null)
            return AuthenticateErrors.NullAssignedRole;
        var result = await userRepository.UpdateRolesAsync(request.UserId, roles);
        if (!result)
            return AuthenticateErrors.RoleAssignmentFailed;

        return ResultMethods.GetResult(result, UpdateSuccess.Roles);
    }
}
