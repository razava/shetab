using Domain.Models.Relational.Common;
using System.ComponentModel.DataAnnotations;

namespace Api.Contracts;


public record NewPasswordDto(
    [Required] [MinLength(6)] [MaxLength(512)]
    string NewPassword);

public record UpdateRolesDto(
    //List<IsInRoleModel> Roles);
    List<IsInRoleDto> Roles);
//todo : no need for roleTitle(or display name) in roles list above
public record IsInRoleDto(
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
    string? Title,
    [MaxLength(64)]
    string? Organization,
    Gender? Gender,
    Education? Education,
    DateTime? BirthDate,
    [MaxLength(11)]
    string? PhoneNumber2,
    //AddressDto Address,
    [MaxLength(16)]
    string? NationalId,
    bool? TwoFactorEnabled, 
    bool? SmsAlert);


public record CreateUserDto(
        [Required] [MaxLength(32)]
        string Username,
        [Required] [MinLength(6)] [MaxLength(512)]
        string Password,
        List<string>? Roles,
        List<int>? RegionIds,
        [MaxLength(32)]
        string FirstName = "",
        [MaxLength(32)]
        string LastName = "",
        [MaxLength(32)]
        string Title = "");


public record CreateComplaintInspectorUserDto(
        [Required]
        int InstanceId,
        [Required] [MaxLength(32)]
        string Username,
        [Required] [MinLength(6)] [MaxLength(512)]
        string Password,
        List<string>? Roles,
        [MaxLength(32)]
        string Title = "");


public record CreateContractorDto(
       [Required] [MaxLength(11)] [Phone]
       string PhoneNumber,
       string FirstName = "",
       string LastName = "",
       string Title = "",
       string Organization = "");




