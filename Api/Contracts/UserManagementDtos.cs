using Api.Dtos;
using Application.Users.Common;

namespace Api.Contracts;


public record NewPasswordDto(
    string NewPassword);


public record UpdateRolesDto(
    //List<IsInRoleModel> Roles);
    List<RolesDto> Roles);
//todo : no need for roleTitle(or display name) in roles list above
public record RolesDto(
    string RoleName,
    string RoleTitle,
    bool IsIn);

public record UpdateUserDto(
    string FirstName,
    string LastName,
    string Title);





