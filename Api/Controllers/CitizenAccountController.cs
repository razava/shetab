﻿using Api.Abstractions;
using Api.Authentication;
using Api.Contracts;
using Api.ExtensionMethods;
using Application.Authentication.Commands.LoginCommand;
using Application.Authentication.Commands.RegisterCitizenCommand;
using Application.Authentication.Commands.ResetPasswordCommand;
using Application.Authentication.Commands.VerifyPhoneNumberCommand;
using Application.Authentication.Queries.ForgotPasswordQuery;
using Application.Authentication.Queries.GetResetPasswordTokenQuery;
using Application.Common.Interfaces.Security;
using Application.Medias.Commands.AddMedia;
using Application.Users.Commands.UpdateUserAvatar;
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
    protected CitizenAccountController(ISender sender) : base(sender)
    {
    }

    

    [HttpPost("Login")]
    public async Task<ActionResult> Login(LoginDto loginDto)
    {
        var mappedCaptcha = loginDto.Captcha.Adapt<CaptchaValidateModel>();

        var command = new LoginCommand(loginDto.Username, loginDto.Password, mappedCaptcha);
        var result = await Sender.Send(command);
        if (result.UserNotConfirmed)
        {
            return StatusCode(StatusCodes.Status428PreconditionRequired, "");
        }
        else
        {
            //TODO: Consider converting token to base64
            return Ok(result.JwtToken);
        }
    }

    [HttpPost("LoginApp")]
    public async Task<ActionResult> LoginApp(LoginAppDto loginAppDto)
    {
        var command = new LoginCommand(loginAppDto.Username, loginAppDto.Password);
        var result = await Sender.Send(command);
        if (result.UserNotConfirmed)
        {
            return StatusCode(StatusCodes.Status428PreconditionRequired, "");
        }
        else
        {
            //TODO: Consider converting token to base64
            return Ok(result.JwtToken);
        }
    }


    [HttpPost("Register")]
    public async Task<ActionResult> Register(RegisterDto registerDto)
    {
        var mappedCaptcha = registerDto.Captcha.Adapt<CaptchaValidateModel>();

        var command = new RegisterCitizenCommand(registerDto.Username, registerDto.Password, mappedCaptcha);
        var result = await Sender.Send(command);
        if (result)
        {
            return StatusCode(StatusCodes.Status428PreconditionRequired, "");
        }
        else
        {
            return BadRequest();
        }
    }

    [HttpPost("RegisterApp")]
    public async Task<ActionResult> RegisterApp(RegisterAppDto registerAppDto)
    {
        var command = new RegisterCitizenCommand(registerAppDto.Username, registerAppDto.Password);
        var result = await Sender.Send(command);
        if (result)
        {
            return StatusCode(StatusCodes.Status428PreconditionRequired, "");
        }
        else
        {
            return BadRequest();
        }
    }

    [HttpPost("Verify")]
    public async Task<ActionResult> Verify(VerificationDto verificationDto)
    {
        var command = new VerifyPhoneNumberCommand(verificationDto.Username, verificationDto.VerificationCode);
        var result = await Sender.Send(command);
        if (result)
        {
            var loginCommand = new LoginCommand(verificationDto.Username, verificationDto.Password);
            var loginResult = await Sender.Send(loginCommand);
            if (loginResult.UserNotConfirmed)
            {
                return Unauthorized();
            }
            else
            {
                return Ok(loginResult.JwtToken);
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
        {
            return Unauthorized();
        }
        var query = new GetUserProfileQuery(userId);
        var result = await Sender.Send(query);
        var mappedUser = result.Adapt<GetCitizenProfileDto>();
        return Ok(mappedUser);
    }

    [Authorize(Roles = "Citizen")]
    [HttpPut]
    public async Task<IActionResult> UpdateUser(UpdateCitizenProfileDto updateCitizenProfileDto)
    {
        await Task.CompletedTask;
        //UpdateUserProfileCommand   //...........................................
        return Ok();//NoContent
    }


    //todo : Define Access Policy

    [Authorize]
    [HttpPut("Avatar")]
    public async Task<ActionResult> UpdateAvatar(UploadDto avatar)
    {
        var userId = User.GetUserId();
        if (userId == null)
        {
            return Unauthorized();
        }
        var command = new UpdateUserAvatarCommand(userId, avatar.File);
        var result = await Sender.Send(command);
        //todo : no need to handle result??
        return NoContent();
    }

    [Authorize(Roles = "Citizen")]
    [HttpPut("Password")]
    public async Task<ActionResult> ChangePassword(ChangePasswordDto changePasswordDto)
    {
        var userName = User.FindFirstValue(ClaimTypes.Name);
        if (userName is null)
        {
            return Unauthorized();
        }

        var mappedCaptcha = changePasswordDto.Captcha.Adapt<CaptchaValidateModel>();
        var command = new ChangePasswordCommand(userName, changePasswordDto.OldPassword, changePasswordDto.NewPassword, mappedCaptcha);
        var result = await Sender.Send(command);
        if (result)
        {
            return NoContent();
        }
        else
        {
            return Problem();
        }
    }

    [Authorize(Roles = "Citizen")]
    [HttpPut("PasswordApp")]
    public async Task<ActionResult> ChangePasswordApp(ChangePasswordAppDto changePasswordAppDto)
    {
        var userName = User.FindFirstValue(ClaimTypes.Name);
        if (userName is null)
        {
            return Unauthorized();
        }
        var command = new ChangePasswordCommand(userName, changePasswordAppDto.OldPassword, changePasswordAppDto.NewPassword);
        var result = await Sender.Send(command);
        if (result)
        {
            return NoContent();
        }
        else
        {
            return Problem();
        }
    }

    [HttpPost("ForgotPasswod")]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordDto forgotPasswordDto)
    {
        var mappedCaptcha = forgotPasswordDto.Captcha.Adapt<CaptchaValidateModel>();
        var query = new ForgotPasswordQuery(forgotPasswordDto.PhoneNumber, mappedCaptcha);
        var result = await Sender.Send(query);
        if (result)
        {
            return StatusCode(StatusCodes.Status428PreconditionRequired, "");
        }
        else
        {
            return BadRequest();
        }
    }

    [HttpPost("ForgotPasswodApp")]
    public async Task<IActionResult> ForgotPasswordApp(ForgotPasswordAppDto forgotPasswordDto)
    {
        var query = new ForgotPasswordQuery(forgotPasswordDto.PhoneNumber);
        var result = await Sender.Send(query);
        if (result)
        {
            return StatusCode(StatusCodes.Status428PreconditionRequired, "");
        }
        else
        {
            return BadRequest();
        }
    }

    [HttpPost("RequestToken")]
    public async Task<ActionResult> RequestToken(RequestTokenDto requestTokenDto)
    {
        var query = new GetResetPasswordTokenQuery(requestTokenDto.PhoneNumber, requestTokenDto.VerificationCode);
        var result = await Sender.Send(query);
        if (result == null)
        {
            return BadRequest();
        }
        return Ok(result);
    }

    [HttpPost("ResetPassword")]
    public async Task<ActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
    {
        var command = new ResetPasswordCommand(resetPasswordDto.Username, resetPasswordDto.ResetPasswordToken, resetPasswordDto.NewPassword);
        var result = await Sender.Send(command);
        if (result)
        {
            return Ok();
        }
        else
        {
            return BadRequest();
        }

    }

    [HttpGet("Captcha")]
    public async Task<ActionResult<CaptchaResultDto>> GetCaptcha()
    {
        var query = new CaptchaQuery();
        var result = await Sender.Send(query);
        var mappedCaptcha = result.Adapt<CaptchaResultDto>();

        return Ok(mappedCaptcha);
    }

    [HttpPost("LoginGov")]
    public async Task<IActionResult> LoginGov(GovLoginDto govLoginDto)
    {
        await Task.CompletedTask;
        return Ok();
    }

}
