using Application.Common.Interfaces.Persistence;
using Application.Common.Statics;
using Application.Users.Common;

namespace Application.Users.Queries.GetUserRoles;

internal class GetRolesQueryHandler(IUserRepository userRepository) : IRequestHandler<GetRolesQuery, Result<List<RoleResponse>>>
{
    public async Task<Result<List<RoleResponse>>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        var roles = await userRepository.GetRoles();
        var result = (await userRepository.GetRoles())
            .Select(role => new RoleResponse(role.Name ?? "", role.Title))
            .ToList();
        result.RemoveAll(p => p.RoleName == RoleNames.PowerUser || p.RoleName == RoleNames.GoldenUser);

        return result;
    }
}
