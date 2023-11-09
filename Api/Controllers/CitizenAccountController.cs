using Api.Abstractions;
using Api.Authentication;
using Api.Contracts;
using Application.Authentication.Commands.LoginCommand;
using Application.Authentication.Commands.RegisterCitizenCommand;
using Application.Common.Interfaces.Security;
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

        var command = new LoginCommand(loginDto.Username, loginDto.Password, mappedCaptcha, loginDto.VerificationCode);
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
        var command = new LoginCommand(loginAppDto.Username, loginAppDto.Password, null, loginAppDto.VerificationCode);
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
        var command = new RegisterCitizenCommand(registerDto.Username, registerDto.Password, registerDto.Captcha);
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
    public async Task<ActionResult> Verify(LoginDto loginDto)
    {
        await Task.CompletedTask;
        return Ok();
    }

    [Authorize(Roles = "Citizen")]
    [HttpGet]
    public async Task<ActionResult<GetUserProfileDto>> GetUser()
    {
        await Task.CompletedTask;
        return Ok();
    }

    [Authorize(Roles = "Citizen")]
    [HttpPut]
    public async Task<IActionResult> UpdateUser(UpdateCitizenProfileDto updateCitizenProfileDto)
    {
        await Task.CompletedTask;
        return Ok();
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
        var command = new ChangePasswordCommand(userName, changePasswordDto.OldPassword, changePasswordDto.NewPassword, changePasswordDto.Captcha);
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

    //todo : fix forgot password process
    [HttpPost("ForgotPasswod")]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordDto forgotPasswordDto)
    {
        await Task.CompletedTask;
        return Ok();
    }

    [HttpPost("ForgotPasswodApp")]
    public async Task<IActionResult> ForgotPasswordApp(ForgotPasswordAppDto forgotPasswordDto)
    {
        await Task.CompletedTask;
        return Ok();
    }

    [HttpPost("RequestToken")]
    public async Task<ActionResult> RequestToken(LoginDto loginDto)
    {
        await Task.CompletedTask;
        return Ok();
    }

    [HttpPost("ResetPassword")]
    public async Task<ActionResult> ResetPassword(LoginDto loginDto)
    {
        await Task.CompletedTask;
        return Ok();
    }

    [HttpGet("Captcha")]
    public string GetCaptcha()
    {
        return "";
    }

    [HttpPost("LoginGov")]
    public async Task<IActionResult> LoginGov(GovLoginDto govLoginDto)
    {
        await Task.CompletedTask;
        return Ok();
    }

    //Dtos
    //todo : move and compelete dtos in another file & write validation
    //public record UpdateUserDto();
    public record GetUserProfileDto();
    //public record ForgotPasswordDto();
    //public record ForgotPasswordAppDto();
    public record GovLoginDto();
    
    
}
