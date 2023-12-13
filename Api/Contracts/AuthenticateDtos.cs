using Domain.Models.Relational.Common;
using System.ComponentModel.DataAnnotations;

namespace Api.Contracts;


public record LoginStaffDto(
    [Required] [MaxLength(32)]
    string Username,
    [Required] [MinLength(6)] [MaxLength(512)]
    string Password);


public record VerificationDto(
    [Required] [MaxLength(11)] [Phone]
    string Username,
    [Required] [MinLength(6)] [MaxLength(512)]
    string Password,
    [Required] [MaxLength(8)]
    string VerificationCode);



public record CaptchaValidateDto(
    Guid Key, 
    [MaxLength(8)]
    string Value);

//public record CaptchaResultDto(
//    Guid Key,
//    byte[] Data);


public record LoginDto(
    [Required] [MaxLength(11)] [Phone]
    string Username,
    [Required] [MinLength(6)] [MaxLength(512)]
    string Password,
    CaptchaValidateDto Captcha);


//[RegularExpression("^09[0-9]{9}$", ErrorMessage = "PhoneNumber Is in Invalid Format.")]
public record LoginAppDto(
    [Required] [MaxLength(11)] [Phone]
    string Username,
    [Required] [MinLength(6)] [MaxLength(512)]
    string Password);


public record RegisterDto(
    [Required] [MaxLength(11)] [Phone]
    string Username,
    [Required] [MinLength(6)] [MaxLength(512)]
    string Password,
    CaptchaValidateDto Captcha);

public record RegisterAppDto(
    [Required] [MaxLength(11)] [Phone]
    string Username,
    [Required] [MinLength(6)] [MaxLength(512)]
    string Password);


public record UpdateCitizenProfileDto(
    [MaxLength(32)]
    string? FirstName,
    [MaxLength(32)]
    string? LastName,
    Gender? Gender,
    Education? Education,
    DateTime? BirthDate,
    [MaxLength(11)]
    string? PhoneNumber2,
    //AddressDto Address,
    [MaxLength(16)]
    string? NationalId);

    
public record UpdateStaffProfileDto(
    [MaxLength(32)]
    string? FirstName,
    [MaxLength(32)]
    string? LastName,
    [MaxLength(11)]
    string? PhoneNumber2,
    [MaxLength(32)]
    string? Title,
    [MaxLength(32)]
    string? Organization,
    //AddressDto Address,
    Education? Education);
 

public record ChangePasswordDto(
    //string Username,
    [Required] [MinLength(6)] [MaxLength(512)]
    string OldPassword,
    [Required] [MinLength(6)] [MaxLength(512)]
    string NewPassword,
    CaptchaValidateDto Captcha);


public record ChangePasswordAppDto(
    [Required] [MinLength(6)] [MaxLength(512)]
    string OldPassword,
    [Required] [MinLength(6)] [MaxLength(512)]
    string NewPassword);


public record ForgotPasswordDto(
    [Required] [MaxLength(11)] [Phone]
    string PhoneNumber,
    CaptchaValidateDto Captcha);

public record ForgotPasswordAppDto(
    [Required] [MaxLength(11)] [Phone]
    string PhoneNumber);


public record RequestTokenDto(
    [Required] [MaxLength(11)] [Phone]
    string PhoneNumber,
    [Required] [MaxLength(8)]
    string VerificationCode);


public record ResetPasswordDto(
    [Required] [MaxLength(11)] [Phone]
    string Username,
    [Required] [MaxLength(1024)]
    string ResetPasswordToken,
    [Required] [MinLength(6)] [MaxLength(512)]
    string NewPassword);



public record GetCitizenProfileDto(
    //string Id,
    string UserName,
    string FirstName,
    string LastName,
    string PhoneNumber,
    string PhoneNumber2,
    string NationalId,
    Gender Gender,
    int Education,
    DateTime? BirthDate,
    AddressDto Address,
    MediaDto Avatar);

public record GetStaffProfileDto(
    string UserName,
    string FirstName,
    string LastName,
    string Title,
    string PhoneNumber,
    int Education,
    AddressDto Address,
    MediaDto Avatar);


//todo : review for annotations
public record GovLoginDto(
    string Code,
    string State);

