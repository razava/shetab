using Api.Abstractions;
using Api.Contracts;
using Api.ExtensionMethods;
using Application.Authentication.Commands.ChangePasswordCommand;
using Application.Authentication.Commands.ChangePhoneNumberCommand;
using Application.Authentication.Commands.LoginCommand;
using Application.Authentication.Commands.RefreshCommand;
using Application.Authentication.Commands.RegisterCitizenCommand;
using Application.Authentication.Commands.ResetPasswordCommand;
using Application.Authentication.Commands.RevokeCommand;
using Application.Authentication.Queries.ChangePhoneNumberQuery;
using Application.Authentication.Queries.ForgotPasswordQuery;
using Application.Authentication.Queries.GetResetPasswordTokenQuery;
using Application.Common.Interfaces.Security;
using Application.Medias.Commands.AddMedia;
using Application.Users.Commands.UpdateUserAvatar;
using Application.Users.Commands.UpdateUserProfile;
using Application.Users.Queries.GetUserProfile;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers;


[Route("api/{instanceId}/[controller]")]
[ApiController]
public class AuthenticateController : ApiController
{
    public AuthenticateController(ISender sender) : base(sender)
    {
    }



    [HttpPost("LoginStaff")]
    public async Task<ActionResult> LoginStaff(LoginStaffDto loginDto)
    {
        var command = new LoginCommand(loginDto.Username, loginDto.Password, loginDto.Captcha);
        var result = await Sender.Send(command);
        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }

    [HttpPost("VerifyStaff")]
    public async Task<ActionResult<LoginResultDto>> Verify([FromBody] StaffVerificationDto verificationDto)
    {
        var command = new TwoFactorLoginCommand(
            verificationDto.OtpToken,
            verificationDto.VerificationCode);
        var result = await Sender.Send(command);
        return result.Match(
            s => Ok(s.Adapt<LoginResultDto>()),
            f => Problem(f));
    }

    [HttpPost("Refresh")]
    public async Task<ActionResult<LoginResultDto>> Refresh([FromBody] RefreshDto refreshDto)
    {
        var command = new RefreshCommand(
            refreshDto.Token,
            refreshDto.RefreshToken);
        var result = await Sender.Send(command);
        return result.Match(
            s => Ok(s.Adapt<LoginResultDto>()),
            f => Problem(f));
    }

    [Authorize]
    [HttpPost("Revoke")]
    public async Task<ActionResult<bool>> Revoke([FromBody] RevokeDto revokeDto)
    {
        var command = new RevokeCommand(
            User.GetUserId(),
            revokeDto.RefreshToken);
        var result = await Sender.Send(command);
        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }

    [HttpPost("LogisterCitizen")]
    public async Task<ActionResult> LogisterCitizen([FromBody] LogisterCitizenDto logisterDto)
    {
        var command = new LogisterCitizenCommand(logisterDto.PhoneNumber, logisterDto.Captcha);
        var result = await Sender.Send(command);
        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }

    [HttpPost("VerifyCitizen")]
    public async Task<ActionResult<LoginResultDto>> VerifyCitizen([FromBody] CitizenVerificationDto logisterDto)
    {
        var command = new TwoFactorLoginCommand(logisterDto.OtpToken, logisterDto.VerificationCode);
        var result = await Sender.Send(command);
        return result.Match(
            s => Ok(s.Adapt<LoginResultDto>()),
            f => Problem(f));
    }

    [Authorize]
    [HttpPut("ChangePassword")]
    public async Task<ActionResult> ChangePassword(ChangePasswordAppDto changePasswordDto)
    {
        var userName = User.FindFirstValue(ClaimTypes.Name);
        if (userName is null)
        {
            return Unauthorized();
        }
        var command = new ChangePasswordCommand(userName, changePasswordDto.OldPassword, changePasswordDto.NewPassword);
        var result = await Sender.Send(command);
        return result.Match(
            s => NoContent(),
            f => Problem(f));
    }

    [Authorize]
    [HttpPost("PhoneNumber")]
    public async Task<ActionResult> ChangePhoneNumberRequest(ChangePhoneNumberRequestDto phoneDto)
    {
        var username = User.GetUserName();
        if (username == null)
            return Unauthorized();

        var command = new ChangePhoneNumberQuery(
            username, phoneDto.NewPhoneNumber, phoneDto.Captcha);

        var result = await Sender.Send(command);
        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }

    [Authorize]
    [HttpPut("PhoneNumber")]
    public async Task<ActionResult> ChangePhoneNumber(ChangePhoneNumberDto phoneDto)
    {
        var username = User.GetUserName();
        if (username == null)
            return Unauthorized();

        var command = new ChangePhoneNumberCommand(
            username,
            phoneDto.Token1,
            phoneDto.Code1,
            phoneDto.Token2,
            phoneDto.Code2);

        var result = await Sender.Send(command);
        return result.Match(
            s => NoContent(),
            f => Problem(f));
    }

    [Authorize(Roles = "Citizen")]
    [HttpGet]
    public async Task<ActionResult<GetCitizenProfileDto>> GetUser()
    {
        var userId = User.GetUserId();
        if (userId == null)
            return Unauthorized();
        var query = new GetUserProfileQuery(userId);
        var result = await Sender.Send(query);

        return result.Match(
            s => Ok(s.Adapt<GetCitizenProfileDto>()),
            f => Problem(f));
    }


    [Authorize(Roles = "Citizen")]
    [HttpPut]
    public async Task<IActionResult> UpdateUser(UpdateCitizenProfileDto updateDto)
    {
        var userId = User.GetUserId();
        if (userId == null)
            return Unauthorized();

        var command = new UpdateUserProfileCommand(
            userId,
            updateDto.FirstName,
            updateDto.LastName,
            null,
            null,
            updateDto.NationalId,
            updateDto.Gender,
            updateDto.Education,
            updateDto.BirthDate,
            updateDto.PhoneNumber2);

        var result = await Sender.Send(command);

        return result.Match(
            s => NoContent(),
            f => Problem(f));
    }


    //todo : Define Access Policy
    [Authorize]
    [HttpPut("Avatar")]
    public async Task<ActionResult> UpdateAvatar([FromForm] UploadDto avatar)
    {
        var userId = User.GetUserId();
        if (userId == null)
        {
            return Unauthorized();
        }
        var command = new UpdateUserAvatarCommand(userId, avatar.File);
        var result = await Sender.Send(command);

        return result.Match(
           s => NoContent(),
           f => Problem(f));
    }


    [Authorize(Roles = "Citizen")]
    [HttpPut("Password")]
    public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
    {
        var userName = User.FindFirstValue(ClaimTypes.Name);
        if (userName is null)
        {
            return Unauthorized();
        }
        var mappedCaptcha = changePasswordDto.Captcha.Adapt<CaptchaValidateModel>();
        var command = new ChangePasswordCommand(userName, changePasswordDto.OldPassword, changePasswordDto.NewPassword, mappedCaptcha);
        var result = await Sender.Send(command);

        return result.Match(
            s => NoContent(),
            f => Problem(f));
    }


    [Authorize(Roles = "Citizen")]
    [HttpPut("PasswordApp")]
    public async Task<ActionResult> ChangePasswordApp([FromBody] ChangePasswordAppDto changePasswordAppDto)
    {
        var userName = User.FindFirstValue(ClaimTypes.Name);
        if (userName is null)
        {
            return Unauthorized();
        }
        var command = new ChangePasswordCommand(userName, changePasswordAppDto.OldPassword, changePasswordAppDto.NewPassword);
        var result = await Sender.Send(command);

        return result.Match(
            s => NoContent(),
            f => Problem(f));
    }


    [HttpPost("ForgotPassword")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
    {
        var mappedCaptcha = forgotPasswordDto.Captcha.Adapt<CaptchaValidateModel>();
        var query = new ForgotPasswordQuery(forgotPasswordDto.PhoneNumber, mappedCaptcha);
        var result = await Sender.Send(query);

        return result.Match(
            s => StatusCode(StatusCodes.Status428PreconditionRequired, ""),
            f => Problem(f));
    }


    [HttpPost("ForgotPasswordApp")]
    public async Task<IActionResult> ForgotPasswordApp([FromBody] ForgotPasswordAppDto forgotPasswordDto)
    {
        var query = new ForgotPasswordQuery(forgotPasswordDto.PhoneNumber);
        var result = await Sender.Send(query);

        return result.Match(
            s => StatusCode(StatusCodes.Status428PreconditionRequired, ""),
            f => Problem(f));
    }


    [HttpPost("RequestToken")]
    public async Task<ActionResult> RequestToken([FromBody] RequestTokenDto requestTokenDto)
    {
        var query = new GetResetPasswordTokenQuery(requestTokenDto.PhoneNumber, requestTokenDto.VerificationCode);
        var result = await Sender.Send(query);

        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }


    [HttpPost("ResetPassword")]
    public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
    {
        var command = new ResetPasswordCommand(resetPasswordDto.Username, resetPasswordDto.ResetPasswordToken, resetPasswordDto.NewPassword);
        var result = await Sender.Send(command);

        return result.Match(
            s => Ok(),
            f => Problem(f));
    }


    [HttpGet("Captcha")]
    public async Task<ActionResult<string>> GetCaptcha()
    {
        var query = new CaptchaQuery();
        var result = await Sender.Send(query);
        if (result is null)
            throw new Exception();
        Response.Headers.Append("Captcha-Key", result.Value.Key.ToString());
        //return "data:image/jpg;base64," + Convert.ToBase64String(result.Data);
        return result.Match(
            s => File(s.Data, "image/jpg"),
            f => Problem(f));
    }


    [HttpPost("LoginGov")]
    public async Task<IActionResult> LoginGov([FromBody] GovLoginDto govLoginDto)
    {
        await Task.CompletedTask;//..................
        return Ok("Not Implemented");
    }

}
