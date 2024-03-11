using Application.Common.Interfaces.Security;
using Domain.Models.Relational.Common;
using System.ComponentModel.DataAnnotations;

namespace Api.Contracts;


public record LoginStaffDto(
    [Required] [MaxLength(32)]
    string Username,
    [Required] [MinLength(6)] [MaxLength(512)]
    string Password,
    CaptchaValidateModel Captcha);

public record LoginResultDto(string JwtToken, string RefreshToken);

public record CitizenVerificationDto(
    string OtpToken,
    [Required] [MaxLength(8)]
    string VerificationCode);

public record StaffVerificationDto(
    string OtpToken,
    [Required] [MaxLength(8)]
    string VerificationCode);

public record LogisterCitizenDto(
    [Required] [RegularExpression(@"^09[0-9]{9}$")]
    string PhoneNumber,
    CaptchaValidateModel Captcha);

public record RevokeDto(
    string RefreshToken);

public record RefreshDto(
    string Token,
    string RefreshToken);

public record ChangePhoneNumberRequestDto(
    [Required] [RegularExpression(@"^09[0-9]{9}$")]
    string NewPhoneNumber,
    CaptchaValidateModel Captcha);

public record ChangePhoneNumberDto(
    string Token1,
    string Code1,
    string Token2,
    string Code2);

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


public record UpdateProfileDto(
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
    bool? TwoFactorEnabled);

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
    [Required]
    string Username,
    CaptchaValidateDto Captcha);

public record ResendOtpDto(
    [Required]
    string OtpToken);

public record ResetPasswordDto(
    string OtpToken,
    string VerificationCode,
    [Required] [MinLength(6)] [MaxLength(512)]
    string NewPassword);


public record GetProfileDto(
    //string Id,
    string UserName,
    string FirstName,
    string LastName,
    string Title,
    string Organization,
    string PhoneNumber,
    string PhoneNumber2,
    string NationalId,
    bool TwoFactorEnabled,
    Gender Gender,
    int Education,
    DateTime? BirthDate,
    AddressDto Address,
    MediaDto Avatar);


//todo : review for annotations
public record GovLoginDto(
    string Code,
    string State);

