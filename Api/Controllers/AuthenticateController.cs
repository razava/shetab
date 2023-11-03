using Api.Abstractions;
using Application.Authentication.Commands.LoginCommand;
using Application.Authentication.Commands.RegisterCitizenCommand;
using Application.Common.Interfaces.Security;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static Api.Controllers.CitizenAccountController;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ApiController
    {
        public AuthenticateController(ISender sender) : base(sender)
        {
        }

        [HttpPost("LoginAdmin")]
        public async Task<ActionResult> LoginAdmin(LoginAdminDto loginAdminDto)
        {
            await Task.CompletedTask;
            return Ok();
        }

        [Authorize] //todo : Roles?? Admin, Manager?
        [HttpPut("Password/{id}")]
        public async Task<IActionResult> ChangePasswordById(string id, ChangePasswordByIdDto changePasswordByIdDto)
        {
            await Task.CompletedTask;
            return Ok();
        }

        [Authorize]
        [HttpGet("Profile")]
        public async Task<ActionResult> GetUserProfile()
        {
            await Task.CompletedTask;
            return Ok();
        }

        [Authorize]
        [HttpPut("Profile")]
        public async Task<ActionResult> UpdateProfile(UpdateUserDto updateUserDto)
        {
            await Task.CompletedTask;
            return Ok();
        }

        [Authorize]
        [HttpGet("Regions")] //{instanceId}
        public async Task<ActionResult> GetRegions()
        {
            await Task.CompletedTask;
            return Ok();
        }





        //.................................

        [HttpPost("Login")]
        public async Task<ActionResult> Login(LoginDto loginDto)
        {
            var command = new LoginCommand(loginDto.Username, loginDto.Password, loginDto.Captcha, loginDto.VerificationCode);
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

        [Authorize]
        [HttpPost("ChangePassword")]
        public async Task<ActionResult> ChangePassword(ChangePasswordDto changePasswordDto)
        {
            var userName = User.FindFirstValue(ClaimTypes.Name);
            if(userName is null)
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

        [Authorize]
        [HttpPost("ChangePasswordApp")]
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
    }
}

public record LoginDto(string Username, string Password, CaptchaValidateModel Captcha, string? VerificationCode = null);
public record LoginAppDto(string Username, string Password, string? VerificationCode = null);
public record RegisterDto(string Username, string Password, CaptchaValidateModel Captcha);
public record RegisterAppDto(string Username, string Password);
public record ChangePasswordDto(string Username, string OldPassword, string NewPassword, CaptchaValidateModel Captcha);
public record ChangePasswordAppDto(string OldPassword, string NewPassword);
public record LoginAdminDto();
public record ChangePasswordByIdDto();