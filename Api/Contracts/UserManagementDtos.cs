using Api.Dtos;
using Application.Users.Common;

namespace Api.Contracts;


public record NewPasswordDto(
    string NewPassword);

public record SetRegionsDto(
     List<int>? RegionIds);

public record UpdateRolesDto(
    //List<IsInRoleModel> Roles);
    List<RolesDto> Roles);
//todo : no need for roleTitle(or display name) in roles list above
public record RolesDto(
    string RoleName,
    string RoleTitle,
    bool IsIn);

public record UpdateUserDto(
    string? FirstName,
    string? LastName,
    string? Title);


public record AdminGetUserList(
    string Id,
    string UserName,
    string FirstName,
    string LastName,
    string Title,
    MediaDto Avatar,
    string PhoneNumber
           /*
           string Organization,
           string PhoneNumber2,
           string NationalId,
           Gender Gender,
           Education Education,
           DateTime? BirthDate,
           AddressDto Address
            */);


public record FilterGetUsers(
    DateTime? SentFromDate,
    DateTime? SentToDate,
    List<string>? RoleNames,
    List<int>? RegionIds,
    string? Query);


public record CreateUserDto(
        string Username,
        string Password,
        string FirstName = "",
        string LastName = "",
        string Title = "");


public record CreateContractorDto(
       string PhoneNumber,
       string Organization = "",
       string FirstName = "",
       string LastName = "",
       string Title = "");



public record GetContractorsList(
    string Id,
    string UserName,
    string FirstName,
    string LastName,
    MediaDto Avatar,
    string Organization);


