﻿using Api.Abstractions;
using Api.Authentication;
using Api.Contracts;
using Api.ExtensionMethods;
using Application.Authentication.Commands.ChangePasswordCommand;
using Application.Authentication.Commands.LoginCommand;
using Application.Authentication.Commands.RegisterCitizenCommand;
using Application.Authentication.Commands.ResetPasswordCommand;
using Application.Authentication.Commands.VerifyPhoneNumberCommand;
using Application.Authentication.Queries.ForgotPasswordQuery;
using Application.Authentication.Queries.GetResetPasswordTokenQuery;
using Application.Common.Interfaces.Security;
using Application.Medias.Commands.AddMedia;
using Application.Users.Commands.UpdateUserAvatar;
using Application.Users.Commands.UpdateUserProfile;
using Application.Users.Queries.GetUserProfile;
using Domain.Models.Relational.IdentityAggregate;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers;


[Route("api/{instanceId}/[controller]")]
[ApiController]
public class CitizenAccountController : ApiController
{
    public CitizenAccountController(ISender sender) : base(sender)
    {
    }

    

    [HttpPost("Login")]
    public async Task<ActionResult> Login([FromBody] LoginDto loginDto)
    {
        var mappedCaptcha = loginDto.Captcha.Adapt<CaptchaValidateModel>();

        var command = new LoginCommand(loginDto.Username, loginDto.Password, mappedCaptcha);
        var result = await Sender.Send(command);

        if (result.IsFailed)
            return Problem(result.ToResult());

        if (result.Value.UserNotConfirmed)
        {
            return StatusCode(StatusCodes.Status428PreconditionRequired, "");
        }
        else
        {
            //TODO: Consider converting token to base64
            return Ok(result.Value.JwtToken);
        }
    }

    [HttpPost("LoginApp")]
    public async Task<ActionResult> LoginApp([FromBody] LoginAppDto loginAppDto)
    {
        var command = new LoginCommand(loginAppDto.Username, loginAppDto.Password);
        var result = await Sender.Send(command);
        
        if (result.IsFailed)
            return Problem(result.ToResult());
        
        if (result.Value.UserNotConfirmed)
        {
            return StatusCode(StatusCodes.Status428PreconditionRequired, "");
        }
        else
        {
            //TODO: Consider converting token to base64
            return Ok(result.Value.JwtToken);
        }
    }


    [HttpPost("Register")]
    public async Task<ActionResult> Register([FromBody] RegisterDto registerDto)
    {
        var mappedCaptcha = registerDto.Captcha.Adapt<CaptchaValidateModel>();

        var command = new RegisterCitizenCommand(registerDto.Username, registerDto.Password, mappedCaptcha);
        var result = await Sender.Send(command);

        return result.Match(
            s => StatusCode(StatusCodes.Status428PreconditionRequired, ""),
            f => Problem(f));
        //if (result)
        //{
        //    return StatusCode(StatusCodes.Status428PreconditionRequired, "");
        //}
        //else
        //{
        //    return BadRequest();
        //}
    }

    [HttpPost("RegisterApp")]
    public async Task<ActionResult> RegisterApp([FromBody] RegisterAppDto registerAppDto)
    {
        var command = new RegisterCitizenCommand(registerAppDto.Username, registerAppDto.Password);
        var result = await Sender.Send(command);

        return result.Match(
            s => StatusCode(StatusCodes.Status428PreconditionRequired, ""),
            f => Problem(f));
    }


    [HttpPost("Verify")]
    public async Task<ActionResult> Verify([FromBody] VerificationDto verificationDto)
    {
        var command = new VerifyPhoneNumberCommand(verificationDto.Username, verificationDto.VerificationCode);
        var result = await Sender.Send(command);
        if (result.IsSuccess)
        {
            var loginCommand = new LoginCommand(verificationDto.Username, verificationDto.Password);
            var loginResult = await Sender.Send(loginCommand);

            if(loginResult.IsFailed)
                return Problem(loginResult.ToResult());

            if (loginResult.Value.UserNotConfirmed)
            {
                return Unauthorized();
            }
            else
            {
                return Ok(loginResult.Value.JwtToken);
            }
        }
        else
        {
            return Unauthorized();
        }
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
        if (result.IsFailed)
            return Problem(result.ToResult());

        Response.Headers.Append("Captcha-Key", result.Value.Key.ToString());
        return Ok("data:image/jpg;base64," + Convert.ToBase64String(result.Value.Data));
    }


    [HttpPost("LoginGov")]
    public async Task<IActionResult> LoginGov([FromBody] GovLoginDto govLoginDto)
    {
        await Task.CompletedTask;//..................
        return Ok("Not Implemented");
    }

}
