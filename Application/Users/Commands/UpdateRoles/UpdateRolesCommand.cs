using Application.Users.Common;

namespace Application.Users.Commands.UpdateRoles;

public record UpdateRolesCommand(string UserId, List<IsInRoleModel> Roles):IRequest<Result<bool>>;
