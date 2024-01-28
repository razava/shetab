using Application.Common.Interfaces.Persistence;
using Application.Users.Common;
using MediatR;

namespace Application.Configurations.Queries.Roles;

internal class GetRolesQueryHandler(IUserRepository userRepository) : IRequestHandler<GetRolesQuery, Result<List<IsInRoleModel>>>
{

    public async Task<Result<List<IsInRoleModel>>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        var result = (await userRepository.GetRoles())
            .Select(role => new IsInRoleModel(role.Name ?? "", role.Title,false))
            .ToList();
        result.RemoveAll(p => p.RoleName == "PowerUser");

        return result;
    }
}
