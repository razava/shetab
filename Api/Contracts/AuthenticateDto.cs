using Api.Dtos;
using Application.Common.Interfaces.Security;
using Domain.Models.Relational.Common;

namespace Api.Contracts;


public record LoginStaffDto(
    string Username,
    string Password);

public record VerificationDto(
    string Username,
    string Password,
    string VerificationCode);



public record CaptchaValidateDto(
    Guid Key, 
    string Value);

public record CaptchaResultDto(
    Guid Key,
    byte[] Data);


public record LoginDto(
    string Username,
    string Password,
    CaptchaValidateDto Captcha);


public record LoginAppDto(
    string Username,
    string Password);


public record RegisterDto(
    string Username,
    string Password,
    CaptchaValidateDto Captcha);

public record RegisterAppDto(
    string Username,
    string Password);


public record UpdateCitizenProfileDto(
    string FirstName,
    string LastName,
    Gender? Gender,
    int? Education,
    DateTime BirthDate,
    string PhoneNumber2,
    AddressDto Address,
    string NationalId);

    
public record UpdateStaffProfileDto(
    string FirstName,
    string LastName,
    string PhoneNumber2,
    string Title,
    AddressDto Address,
    int? EducationId);


public record ChangePasswordDto(
    string Username,
    string OldPassword,
    string NewPassword,
    CaptchaValidateModel Captcha);


public record ChangePasswordAppDto(
    string OldPassword,
    string NewPassword);


public record ForgotPasswordDto(
    string PhoneNumber,
    CaptchaValidateDto Captcha);

public record ForgotPasswordAppDto(
    string PhoneNumber);


public record RequestTokenDto(
    string PhoneNumber,
    string VerificationCode);


public record ResetPasswordDto(
    string Username,
    string ResetPasswordToken,
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




public record GovLoginDto(
    string Code,
    string State);

