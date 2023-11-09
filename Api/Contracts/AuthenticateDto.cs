using Api.Dtos;
using Application.Common.Interfaces.Security;
using Domain.Models.Relational.Common;

namespace Api.Contracts;


public record CaptchaValidateDto(
    Guid Key, 
    string Value);


public record LoginDto(
    string Username,
    string Password,
    CaptchaValidateDto Captcha,
    string? VerificationCode = null); //todo : remove??


public record LoginAppDto(
    string Username,
    string Password,
    string? VerificationCode = null); //todo : remove??


public record RegisterDto(
    string Username,
    string Password,
    CaptchaValidateModel Captcha);

public record RegisterAppDto(
    string Username,
    string Password);


public record UpdateCitizenProfileDto(
    string FirstName,
    string LastName,
    Gender? Gender,
    int? EducationId,
    DateTime BirthDate,
    string PhoneNumber2,
    AddressDto Address,
    string NationalId);


public record UpdateProfileDto(
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
    Guid CaptchaKey,
    string CaptchaValue);

public record ForgotPasswordAppDto(
    string PhoneNumber);


public record GovLoginDto();






