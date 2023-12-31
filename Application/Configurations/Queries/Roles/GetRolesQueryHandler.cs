using Application.Common.Interfaces.Persistence;
using Application.Users.Common;
using MediatR;

namespace Application.Configurations.Queries.Roles;

internal class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, List<IsInRoleModel>>
{
    private readonly IUserRepository _userRepository;

    public GetRolesQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    public async Task<List<IsInRoleModel>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        var result = (await _userRepository.GetRoles())
            .Select(role => new IsInRoleModel(role.Name ?? "", role.Title,false))
            .ToList();
        result.RemoveAll(p => p.RoleName == "PowerUser");

        return result;
    }
}
