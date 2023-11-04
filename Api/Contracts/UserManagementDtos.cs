using Api.Dtos;
using Application.Users.Common;

namespace Api.Contracts;


public record NewPasswordDto(
    string NewPassword
);


public record UpdateRolesDto(
    List<IsInRoleModel> Roles
);
//todo : no need for roleTitle(or display name) in roles list above
public record RoleDto(
    string RoleName,
    bool IsIn
);
