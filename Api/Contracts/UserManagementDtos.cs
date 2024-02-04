using Api.Dtos;
using Application.Users.Common;
using Domain.Models.Relational.Common;
using System.ComponentModel.DataAnnotations;

namespace Api.Contracts;


public record NewPasswordDto(
    [Required] [MinLength(6)] [MaxLength(512)]
    string NewPassword);

public record SetRegionsDto(
     List<int>? RegionIds);

public record UpdateRolesDto(
    //List<IsInRoleModel> Roles);
    List<RolesDto> Roles);
//todo : no need for roleTitle(or display name) in roles list above
public record RolesDto(
    [Required] [MaxLength(32)]
    string RoleName,
    [MaxLength(32)]
    string RoleTitle,
    [Required]
    bool IsIn);

public record IsInRegionDto(
    [Required]
    int RegionId,
    [MaxLength(64)]
    string RegionName,
    [Required]
    bool IsIn);


public record UpdateUserCategories(
    List<int> CategoryIds);


public record UpdateUserDto(
    [MaxLength(32)]
    string? FirstName,
    [MaxLength(32)]
    string? LastName,
    [MaxLength(32)]
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


public record AdminGetUserDetailsDto(
    string Id,
    string UserName,
    string FirstName,
    string LastName,
    string Title,
    MediaDto Avatar,
    string PhoneNumber,
    string Organization,
    string PhoneNumber2,
    string NationalId,
    Gender Gender,
    Education Education,
    DateTime? BirthDate,
    AddressDto Address);


public record CreateUserDto(
        [Required] [MaxLength(32)]
        string Username,
        [Required] [MinLength(6)] [MaxLength(512)]
        string Password,
        [MaxLength(32)]
        string FirstName = "",
        [MaxLength(32)]
        string LastName = "",
        [MaxLength(32)]
        string Title = "");


public record CreateContractorDto(
       [Required] [MaxLength(11)] [Phone]
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


public record GetCitizenDto(
    string UserName,
    string FirstName,
    string LastName,
    MediaDto Avatar,
    string PhoneNumber,
    AddressDetailDto Address);

